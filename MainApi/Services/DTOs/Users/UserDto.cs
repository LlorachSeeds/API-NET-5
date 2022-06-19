using System;
using System.ComponentModel.DataAnnotations;
using Domain.Roles;
using Services.DTOs.Persons;
using Services.DTOs.Roles;

namespace Services.DTOs.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        public bool IsActive { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FirstLastName { get; set; }

        public string SecondLastName { get; set; }

        public DateTime? DateBorn { get; set; }

        public RolDto Rol { get; set; }

        public PersonDto Person { get; set; }
    }
}