#nullable enable
using System;
using Domain.Users;

namespace Domain.Persons
{
    public class Person
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FirstLastName { get; set; }

        public string SecondLastName { get; set; }

        public string Sex { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Identification { get; set; }

        public DateTime DateBorn { get; set; }


        public Guid UserId { get; set; }

        public User User { get; set; }

        public bool IsDeleted { get; set; }

        public Person()
        {
        }

        public Person(string firstName, string secondName, string firstLastName, string secondLastName, string sex, string email, string phoneNumber, string identification, DateTime dateBorn)
        {
            FirstName = firstName;
            SecondName = secondName;
            FirstLastName = firstLastName;
            SecondLastName = secondLastName;
            Sex = sex;
            Email = email;
            PhoneNumber = phoneNumber;
            Identification = identification;
            DateBorn = dateBorn;
        }
    }
}
