using System;

namespace Services.DTOs.Persons
{
    public class PersonDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FirstLastName { get; set; }

        public string SecondLastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Identification { get; set; }

        public string Sex { get; set; }

        public string Address { get; set; }
    }
}