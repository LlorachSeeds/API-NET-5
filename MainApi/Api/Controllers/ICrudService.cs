using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public interface ICrudService<TEntity, TEntityDto, TCreateUpdateEntityDto>
    {
        public Task<ActionResult<TEntityDto>> GetElementById([FromHeader] Guid id);

        public Task<ActionResult<TEntityDto>> CreateElement([FromBody] TCreateUpdateEntityDto createUpdateEntityDto);

        public Task<ActionResult<TEntityDto>> UpdateElement([FromBody] TCreateUpdateEntityDto createUpdateEntityDto);

        public Task<ActionResult<TEntityDto>> GetEntityList();

        public Task<ActionResult> DeleteEntity([FromHeader] Guid id);
    }
}