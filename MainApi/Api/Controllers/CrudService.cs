using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.REPs;
using Services.SRVs;
using Services.UTLs;

namespace Api.Controllers
{
    public abstract class CrudService<TEntity, TEntityDto, TCreateUpdateEntityDto> : ICrudService<TEntity, TEntityDto, TCreateUpdateEntityDto>
        where TEntity : class
    {
        protected readonly IService<TEntity, TEntityDto, TCreateUpdateEntityDto> Service;

        // protected readonly ServiceRepository<TEntity, TEntityDto, TCreateUpdateEntityDto> ServiceRepository;
        public CrudService(IService<TEntity, TEntityDto, TCreateUpdateEntityDto> service)
        {
            Service = service;

            // ServiceRepository = new ServiceRepository<TEntity, TEntityDto, TCreateUpdateEntityDto>(context);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(CustomResult))]
        [ProducesResponseType(400, Type = typeof(CustomResult))]
        [ProducesResponseType(500, Type = typeof(CustomResult))]
        public virtual async Task<ActionResult<TEntityDto>> GetElementById([FromHeader]Guid id)
        {
            ActionResult result = new BadRequestResult();

            try
            {
                if (id != Guid.Empty)
                {
                    TEntityDto entityDto = await Service.FetchAsync(id);
                    if (entityDto != null)
                    {
                        result = new OkObjectResult(entityDto);
                    }
                }
            }
            catch (Exception e)
            {
                result = Utilities.CreateObjectResult("GetElementById", e);
            }

            return result;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(CustomResult))]
        [ProducesResponseType(400, Type = typeof(CustomResult))]
        [ProducesResponseType(500, Type = typeof(CustomResult))]
        public virtual async Task<ActionResult<TEntityDto>> CreateElement([FromBody]TCreateUpdateEntityDto createUpdateEntityDto)
        {
            ActionResult result = new BadRequestResult();

            try
            {
                if (createUpdateEntityDto != null)
                {
                    TEntityDto entityDto = await Service.InsertAsync(createUpdateEntityDto);

                    if (entityDto != null)
                    {
                        result = new OkObjectResult(entityDto);
                    }
                }
            }
            catch (Exception e)
            {
                result = Utilities.CreateObjectResult("CreateElement", e);
            }

            return result;
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(CustomResult))]
        [ProducesResponseType(400, Type = typeof(CustomResult))]
        [ProducesResponseType(500, Type = typeof(CustomResult))]
        public virtual async Task<ActionResult<TEntityDto>> UpdateElement([FromBody]TCreateUpdateEntityDto createUpdateEntityDto)
        {
            ActionResult result = new BadRequestResult();

            try
            {
                if (createUpdateEntityDto != null)
                {
                    TEntityDto updatedElement = await Service.RefreshAsync(createUpdateEntityDto);
                    if (updatedElement != null)
                    {
                        result = new OkObjectResult(updatedElement);
                    }
                }
            }
            catch (Exception e)
            {
                result = Utilities.CreateObjectResult("UpdateElement", e);
            }

            return result;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(CustomResult))]
        [ProducesResponseType(400, Type = typeof(CustomResult))]
        [ProducesResponseType(500, Type = typeof(CustomResult))]
        public virtual async Task<ActionResult<TEntityDto>> GetEntityList()
        {
            ActionResult result = new BadRequestResult();

            List<TEntityDto> listEntityDto = await Service.FetchManyAsync();

            if (listEntityDto != null)
            {
                result = new OkObjectResult(listEntityDto);
            }

            return result;
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(CustomResult))]
        [ProducesResponseType(400, Type = typeof(CustomResult))]
        [ProducesResponseType(500, Type = typeof(CustomResult))]
        public virtual async Task<ActionResult> DeleteEntity([FromHeader] Guid id)
        {
            ActionResult result = new BadRequestResult();

            try
            {
                bool isDeleted = await Service.RemoveAsync(id);
                if (isDeleted)
                {
                    result = new OkResult();
                }
            }
            catch (Exception e)
            {
                result = Utilities.CreateObjectResult("DeleteEntity", e);
            }

            return result;
        }
    }
}