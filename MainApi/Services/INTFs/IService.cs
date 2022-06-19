namespace Services.INTFs
{
    public interface IService<TEntity, TEntityDto, TCreateUpdateDto>
        : IServiceRepository<TEntity, TEntityDto, TCreateUpdateDto>
    {
    }
}