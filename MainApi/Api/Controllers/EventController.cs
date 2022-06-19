using DbAccess.Context;
using Domain.Events;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs.Events;
using Services.SRVs;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class EventController : CrudService<Event, EventDto, CreateUpdateEventDto>
    {
        public EventController(IService<Event, EventDto, CreateUpdateEventDto> service)
            : base(service)
        {
        }
    }
}