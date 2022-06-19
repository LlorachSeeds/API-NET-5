using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Domain.Persons;
using Domain.Roles;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs.Persons;
using Services.DTOs.Roles;
using Services.DTOs.Users;

namespace Services.UTLs
{
    public class Utilities
    {
        private static MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<Person, PersonDto>();
            cfg.CreateMap<User, UserDto>()
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(e => e.Person.FirstName))
                .ForMember(dest => dest.SecondName, opts => opts.MapFrom(e => e.Person.SecondName))
                .ForMember(dest => dest.FirstLastName, opts => opts.MapFrom(e => e.Person.FirstLastName))
                .ForMember(dest => dest.SecondLastName, opts => opts.MapFrom(e => e.Person.SecondLastName))
                .ForMember(dest => dest.PhoneNumber, opts => opts.MapFrom(e => e.Person.PhoneNumber))
                .ForMember(dest => dest.DateBorn, opts => opts.MapFrom(e => e.Person.DateBorn))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(e => e.Person.Email));
            cfg.CreateMap<CreateUpdateUserDto, User>();
            cfg.CreateMap<CreateUpdatePersonDto, Person>();
            cfg.CreateMap<Rol, RolDto>();
        });

        public static Mapper Mapper { get; set; } = new(config);

        #region HELPERS

        public string GetSha256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }

            return sb.ToString();
        }

        public bool SendEmail(string emailType, string emailTo, string subject, string body)
        {
            try
            {
                string emailFrom;
                string emailPassw;

                // correo para manejo de cuentas de usuario
                if (emailType == "EmailAccounts")
                {
                    emailFrom = "ChevacaAgrotechnology@gmail.com";
                    emailPassw = "Chev1122aca33";
                }

                // otros if para otras cuentas y que salga de appconfig o la bd
                else
                {
                    return false;
                }

                MailMessage oMailMessage = new(emailFrom, emailTo, subject, body);

                oMailMessage.IsBodyHtml = true;

                SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
                oSmtpClient.EnableSsl = true;
                oSmtpClient.UseDefaultCredentials = false;
                oSmtpClient.Port = 587;
                oSmtpClient.Credentials = new System.Net.NetworkCredential(emailFrom, emailPassw);

                oSmtpClient.Send(oMailMessage);

                oSmtpClient.Dispose();
                return true;
            }
            catch (System.Exception ex)
            {
                // loguear el error
                return false;
            }
        }

        public static ObjectResult CreateObjectResult(string methodAffected, Exception exc)
        {
            ObjectResult result;
            if (exc is ErrorException)
            {
                ErrorException exception = (ErrorException)exc;
                result = new ObjectResult(new CustomResult(exception));
            }
            else
            {
                ErrorException errorException = new ErrorException(StatusCodes.Status500InternalServerError, methodAffected, exc.Message);
                result = new ObjectResult(new CustomResult(errorException)) { StatusCode = errorException.Code };
            }

            return result;
        }

        #endregion
    }
}