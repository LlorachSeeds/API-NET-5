using Services.REPs;

namespace Services.SRVs
{
    public interface IService<TEntity, TEntityDto, TCreateUpdateDto> : IServiceRepository<TEntity, TEntityDto, TCreateUpdateDto>
    {
    }
}