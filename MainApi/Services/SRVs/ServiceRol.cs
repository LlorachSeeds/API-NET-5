using System.Linq;
using System.Threading.Tasks;
using DbAccess.Context;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.DTOs.Roles;
using Services.REPs;
using Services.UTLs;

namespace Services.SRVs
{
    public class ServiceRol : ServiceRepository<Rol, RolDto, CreateUpdateRolDto>, IService<Rol, RolDto, CreateUpdateRolDto>
    {
        public ServiceRol(Context context, IConfiguration configuration)
            : base(context, configuration)
        {
        }

        public async Task<RolDto> FetchRolByName(string rolName)
        {
            // To Do: Lanzar Excepcion en caso de error inesperado
            RolDto rolDto = null;
            if (!string.IsNullOrWhiteSpace(rolName))
            {
                Rol rol = await GetRolByName(rolName);
                if (rol != null)
                {
                    rolDto = Utilities.Mapper.Map<RolDto>(rol);
                }
            }

            return rolDto;
        }

        public async Task<Rol> GetRolByName(string rolName)
        {
            // To Do: Lanzar Excepcion en caso de error inesperado
            Rol searchedRol = null;
            if (!string.IsNullOrWhiteSpace(rolName))
            {
                searchedRol = await DbSet.SingleOrDefaultAsync(e => e.RolName.Equals(rolName));
            }

            return searchedRol;
        }
    }
}