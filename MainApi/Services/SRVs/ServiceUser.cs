using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DbAccess.Context;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Services.DTOs.Sessions;
using Services.DTOs.Users;
using Services.REPs;
using Services.UTLs;

namespace Services.SRVs
{
    public class ServiceUser : ServiceRepository<User, UserDto, CreateUpdateUserDto>,
        IService<User, UserDto, CreateUpdateUserDto>
    {
        private readonly ServiceRol _serviceRol;

        public ServiceUser(Context context, IConfiguration configuration, ServiceRol serviceRol)
            : base(context, configuration)
        {
            _serviceRol = serviceRol;
        }

        public async Task<SessionDto> Login(LoginDto creds)
        {
            SessionDto sessionDto = null;

            if (!string.IsNullOrWhiteSpace(creds.Password) && !string.IsNullOrWhiteSpace(creds.Username))
            {
                User user = await FetchAsyncByEmail(creds.Username);
                if (user != null)
                {
                    bool passVerify = BCrypt.Net.BCrypt.Verify(creds.Password, user.Password);

                    if (passVerify)
                    {
                        // To Do: Hacer login en Chirpstack y guardar el token en el ususario
                        UserDto userDto = Utilities.Mapper.Map<UserDto>(user);
                        JwtDto token = CreateJwt(user.Person.Email);

                        if (token != null && userDto != null)
                        {
                            sessionDto = new SessionDto(token, userDto);
                        }
                    }
                }
            }

            return sessionDto;
        }

        private async Task<User> FetchAsyncByEmail(string email)
        {
            User user = null;

            if (!string.IsNullOrWhiteSpace(email))
            {
                user = await DbSet.Include(e => e.Person).Where(e => e.Person.Email.Equals(email)).SingleOrDefaultAsync();
                if (user == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, "User", Errors.ElementNotFound);
                }
            }

            return user;
        }

        public JwtDto CreateJwt(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("Email", email)
                };
                DateTime expirationDate = DateTime.Now.AddDays(1);

                string tokenJwt = TokenConstructor(claims, expirationDate);

                JwtDto answer = new JwtDto()
                {
                    Token = tokenJwt,
                    Expiration = expirationDate
                };

                return answer;
            }

            throw new Exception(Errors.UnknownError);
        }

        public string TokenConstructor(List<Claim> claimList, DateTime expiration)
        {
            if (claimList != null && expiration > DateTime.Now)
            {
                string answer = string.Empty;
                if (claimList != null && expiration > DateTime.Today)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    JwtSecurityToken securityToken =
                        new JwtSecurityToken(issuer: null, audience: null, claims: claimList, expires: expiration, signingCredentials: creds);

                    answer = new JwtSecurityTokenHandler().WriteToken(securityToken);
                }

                return answer;
            }

            throw new Exception(Errors.UnknownError);
        }

        public async Task<bool> CheckUserExistByEmail(string email)
        {
            bool exist = false;

            try
            {
                User searchedUser = await FetchAsyncByEmail(email);
                if (searchedUser != null)
                {
                    exist = true;
                }
            }
            catch (Exception e)
            {
                exist = false;
            }

            return exist;
        }

        public override async Task<List<User>> GetListAsync()
        {
            return await DbSet
                .Include(e => e.Person)
                .Include(e => e.Rol)
                .ToListAsync();
        }

        public async Task<UserDto> InsertRootUserAsync(CreateUpdateUserDto createUpdateEntityDto)
        {
            CreateUpdateUserDto remoteUser = await CreateRemoteUser(createUpdateEntityDto);
            remoteUser.Rol = await _serviceRol.FetchRolByName(createUpdateEntityDto.RolName);
            return await InsertAsync(createUpdateEntityDto);
        }

        public async Task<CreateUpdateUserDto> CreateRemoteUser(CreateUpdateUserDto createUpdateUserDto)
        {
            CreateUpdateUserDto userWithExternalData = createUpdateUserDto;
            HttpClient client = new HttpClient();
            Uri url = new Uri(ChirpstackApiRoute + "/users");
            Task<HttpResponseMessage> response = client.PostAsJsonAsync(url, userWithExternalData);
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                string responseContent = await response.Result.Content.ReadAsStringAsync();
                userWithExternalData = JsonConvert.DeserializeObject<CreateUpdateUserDto>(responseContent);
            }

            return userWithExternalData;
        }
    }
}