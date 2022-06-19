using System;

namespace Services.DTOs.Roles
{
    public class RolDto
    {
        public Guid Id { get; set; }

        public string RolName { get; set; }

        public string Description { get; set; }

        public RolDto()
        {
        }

        public RolDto(string rolName, string description)
        {
            RolName = rolName;
            Description = description;
        }
    }
}