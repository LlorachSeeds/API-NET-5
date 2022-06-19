using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs.Sessions;
using Services.DTOs.Users;
using Services.SRVs;
using Services.UTLs;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : CrudService<User, UserDto, CreateUpdateUserDto>
    {
        ServiceUser ServiceUser { get; set; }

        public UserController(IService<User, UserDto, CreateUpdateUserDto> service)
            : base(service)
        {
            ServiceUser = (ServiceUser)Service;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(CustomResult))]
        [ProducesResponseType(400, Type = typeof(CustomResult))]
        [ProducesResponseType(500, Type = typeof(CustomResult))]
        public async Task<ActionResult<SessionDto>> Login([FromBody]LoginDto creds)
        {
            ActionResult result = null;
            var traceId = Activity.Current?.Id;

            try
            {
                SessionDto sessionDto = await ServiceUser.Login(creds);
                if (sessionDto != null)
                {
                    result = new OkObjectResult(sessionDto);
                }
            }
            catch (Exception e)
            {
                if (e is ErrorException)
                {
                    ErrorException exception = (ErrorException)e;
                    result = new ObjectResult(new CustomResult(exception));
                }
                else
                {
                    ErrorException errorException = new ErrorException(StatusCodes.Status500InternalServerError, "Login", e.Message);
                    result = new ObjectResult(new CustomResult(errorException)) { StatusCode = errorException.Code };
                }
            }

            return result;
        }

        [HttpPost("CheckEmailExist")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(CustomResult))]
        [ProducesResponseType(400, Type = typeof(CustomResult))]
        [ProducesResponseType(500, Type = typeof(CustomResult))]
        public async Task<ActionResult<bool>> CheckEmailExist([FromHeader][Required] string email)
        {
            ActionResult result = null;
            var traceId = Activity.Current?.Id;
            bool exist = false;

            try
            {
                exist = await ServiceUser.CheckUserExistByEmail(email);
                result = new OkObjectResult(exist);
            }
            catch (Exception e)
            {
                return Utilities.CreateObjectResult("CheckEmailExist", e);
            }

            return result;
        }

        [HttpPost("RootUser")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(CustomResult))]
        [ProducesResponseType(400, Type = typeof(CustomResult))]
        [ProducesResponseType(500, Type = typeof(CustomResult))]
        public async Task<ActionResult<UserDto>> CreateRootUser([FromBody] CreateUpdateUserDto createUpdateUserDto)
        {
            ActionResult result = null;

            try
            {
                UserDto userCreated = await ServiceUser.InsertRootUserAsync(createUpdateUserDto);
                if (userCreated != null)
                {
                    result = new OkObjectResult(userCreated);
                }
            }
            catch (Exception e)
            {
                return Utilities.CreateObjectResult("CreateRootUser", e);
            }

            return result;
        }
    }
}