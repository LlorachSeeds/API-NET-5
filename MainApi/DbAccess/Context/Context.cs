using System.Linq;
using Domain.Persons;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<UserSettings> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Users");
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.HasOne(e => e.Settings)
                    .WithOne(e => e.User)
                    .HasForeignKey<User>(e => e.UserSettingsId);
            });

            builder.Entity<Person>(entity =>
            {
                entity.ToTable("Persons");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Persons");
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SecondName)
                    .IsRequired(false);

                entity.Property(e => e.SecondLastName)
                    .IsRequired(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired(false);
                entity.Property(e => e.Address)
                    .IsRequired(false);

                entity.HasOne(e => e.User)
                    .WithOne(e => e.Person)
                    .HasForeignKey<Person>(e => e.UserId);
            });

            foreach (var foreignKey in builder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}