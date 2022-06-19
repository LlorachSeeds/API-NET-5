using System;
using Api.Controllers;
using Api.Tests.Mocks;
using DbAccess.Context;
using Domain.Persons;
using Domain.Roles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.SRVs;

namespace ApiTests.TestObjects
{
    public static class ControllerFactory
    {
        private static Context Context { get; set; }
        private static IConfiguration ConfigMock { get; set; }
        
        static ControllerFactory()
        {
            var dbName = Guid.NewGuid().ToString();
            Context contextOne = PreloadedContext(dbName);
            Context = ContextConstructor(dbName);
            
            ConfigMock = new ConfigurationMock();
        }

        static Context ContextConstructor( string dbName)
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(dbName).Options;
            var dbContext = new Context(options);

            return dbContext;
        }

        static Context PreloadedContext(string contextName)
        {
            Context context = ContextConstructor(contextName);
            Rol role = new("Admin", String.Empty);
            Person person = new Person("Pablo", "Enrique", "Llorach", "Paz", "M", "llorach.pablo@llorachdevs.com", "+59891211845", "42883341", new DateTime(1984, 11, 03)); 
            
            context.Persons.Add(person);
            context.SaveChanges();

            return context;
        }

        public static UserController CreateUserController()
        {
            ServiceRol serviceRol = new ServiceRol(Context, ConfigMock);
            ServiceUser serviceUser = new ServiceUser(Context, ConfigMock, serviceRol);
            UserController userController = new UserController(serviceUser);

            return userController;
        }
    }
}