using System;
using Domain.Persons;
using Domain.Roles;
using Domain.Users;
using Microsoft.Extensions.Configuration;

namespace DbAccess.Context
{
    public class DbInitializer
    {
        private readonly Context _context;
        private readonly IConfiguration _configuration;

        public DbInitializer(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void InitializeData()
        {
            bool init = _context.Database.EnsureCreated();

            if (init)
            {
                /* ------------ REPORTS DAILY EMAIL TESTING ------------ */
                /* ------------ persons, users, reports ------------ */

                Rol clientRol = new Rol("RanchUser", string.Empty);
                Rol ranchAdminRol = new Rol("RanchAdmin", string.Empty);
                Rol rootRol = new Rol("Root", string.Empty);
                _context.Add(clientRol);
                _context.Add(ranchAdminRol);
                _context.Add(rootRol);
                _context.SaveChanges();

                UserSettings userOneSettings = new UserSettings();
                UserSettings llorachSettings = new UserSettings();

                User llorachUser = new User();
                llorachUser.UserName = "pablo.llorach";
                llorachUser.Rol = rootRol;
                llorachUser.Settings = llorachSettings;
                llorachUser.Password = BCrypt.Net.BCrypt.HashPassword("03111984");
                llorachUser.IsActive = false;

                Person llorachPerson = new Person();
                llorachPerson.FirstName = "Pablo";
                llorachPerson.FirstLastName = "Llorach";
                llorachPerson.Email = "llorach.pablo@gmail.com";
                llorachPerson.Identification = "42883341";
                llorachPerson.PhoneNumber = "+59891211845";
                llorachPerson.DateBorn = new DateTime(1984, 11, 3);
                llorachPerson.Address = "Agraciada 2929";
                llorachPerson.Sex = "M";
                llorachPerson.User = llorachUser;

                _context.Persons.Add(llorachPerson);
                _context.SaveChanges();

                LoadSpInMainDb();
            }
        }

        public void InitializeFirstRunData()
        {
            // PRECARGA TIPOS DE INCIDENTES
        }

        public void Presentation()
        {
        }

        public void LoadSpInMainDb()
        {
        }
    }
}