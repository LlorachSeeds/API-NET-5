using System;
using System.ComponentModel.DataAnnotations;
using Services.DTOs.Persons;
using Services.DTOs.Roles;

namespace Services.DTOs.Users
{
    public class CreateUpdateUserDto
    {
        [Required(AllowEmptyStrings = true)]
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string RolName { get; set; }

        public RolDto Rol { get; set; }

        public CreateUpdatePersonDto Person { get; set; }

        public CreateUpdateUserDto()
        {
        }

        public CreateUpdateUserDto(string email, string username, string rolName, RolDto rol, CreateUpdatePersonDto person)
        {
            Email = email;
            Username = username;
            RolName = rolName;
            Rol = rol;
            Person = person;
        }
    }
}