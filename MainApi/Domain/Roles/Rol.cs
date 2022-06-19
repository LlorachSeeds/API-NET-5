using System;
using System.Collections.Generic;
using Domain.Users;

namespace Domain.Roles
{
    public class Rol
    {
        public Guid Id { get; set; }

        public string RolName { get; set; }

        public string Description { get; set; }

        public List<User> Users { get; set; }

        public Rol()
        {
            Users = new List<User>();
        }

        public Rol(string name, string description)
        {
            RolName = name;
            Description = description;
            Users = new List<User>();
        }
    }
}