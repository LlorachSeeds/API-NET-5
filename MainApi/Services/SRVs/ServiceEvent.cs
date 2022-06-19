using DbAccess.Context;
using Domain.Events;
using Microsoft.Extensions.Configuration;
using Services.DTOs.Events;
using Services.REPs;

namespace Services.SRVs
{
    public class ServiceEvent : ServiceRepository<Event, EventDto, CreateUpdateEventDto>, IService<Event, EventDto, CreateUpdateEventDto>
    {
        public ServiceEvent(Context context, IConfiguration configuration)
            : base(context, configuration)
        {
        }
    }
}