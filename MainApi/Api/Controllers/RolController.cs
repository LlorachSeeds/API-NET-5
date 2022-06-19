using DbAccess.Context;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs.Roles;
using Services.SRVs;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class RolController : CrudService<Rol, RolDto, CreateUpdateRolDto>
    {
        public RolController(IService<Rol, RolDto, CreateUpdateRolDto> service)
            : base(service)
        {
        }
    }
}