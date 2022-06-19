using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DbAccess.Context
{
    public class DesignTimeContext : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<Context>();

            var conn = new SqlConnection(configuration["ConnectionStringForMigrationsInProductionEnvMigration"]);
            optionsBuilder.UseSqlServer(conn);

            return new Context(optionsBuilder.Options);
        }
    }
}