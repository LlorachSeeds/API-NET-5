using System;
using System.ComponentModel.DataAnnotations;

namespace Services.DTOs.Persons
{
    public class CreateUpdatePersonDto
    {
        [Required(AllowEmptyStrings = true)]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string FirstLastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}