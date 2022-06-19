using System.Threading.Tasks;
using DbAccess.Context;
using Domain.Persons;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs.Persons;
using Services.SRVs;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PersonController : CrudService<Person, PersonDto, CreateUpdatePersonDto>
    {
        public PersonController(IService<Person, PersonDto, CreateUpdatePersonDto> service)
            : base(service)
        {
        }
    }
}