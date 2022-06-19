using DbAccess.Context;
using Domain.Events;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs.Events;
using Services.SRVs;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class EventTypeController : CrudService<EventType, EventTypeDto, CreateUpdateEventTypeDto>
    {
        public EventTypeController(IService<EventType, EventTypeDto, CreateUpdateEventTypeDto> service)
            : base(service)
        {
        }
    }
}