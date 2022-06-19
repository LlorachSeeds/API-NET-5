using System;
using DbAccess.Context;
using Domain.Roles;
using Domain.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Services.DTOs.Roles;
using Services.DTOs.Users;
using Services.SRVs;

namespace Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            if (_env.IsDevelopment())
            {
                bool isRemoteDb = bool.Parse(_configuration["remoteDb"]);
                if (isRemoteDb)
                {
                    services.AddDbContext<Context>(e => e.UseSqlServer(_configuration["RemoteConnectionString"]));
                }
                else
                {
                    services.AddDbContext<Context>(e => e.UseSqlServer(_configuration["ConnectionString"]));
                }
            }
            else if (_env.IsProduction())
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    services.AddDbContext<Context>(e => e.UseSqlServer(connectionString));
                }
            }

            services.AddTransient<IService<User, UserDto, CreateUpdateUserDto>, ServiceUser>();
            services.AddTransient<IService<Rol, RolDto, CreateUpdateRolDto>, ServiceRol>();
            services.AddTransient<ServiceRol>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(e =>
            {
                e.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader().AllowAnyMethod();

                e.AllowAnyOrigin();
                e.AllowAnyMethod();
                e.AllowAnyHeader();
            });

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<Context>();
                var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();

                if (_env.IsDevelopment())
                {
                    bool isFirstRun = bool.Parse(_configuration["firstRun"]);

                    if (isFirstRun)
                    {
                        // new DbInitializer(context, configuration).InitializeFirstRunData();
                        new DbInitializer(context, configuration).Presentation();
                    }
                    else
                    {
                        new DbInitializer(context, configuration).InitializeData();
                    }
                }
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
