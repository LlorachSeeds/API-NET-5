using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.REPs
{
    public interface IServiceRepository<TEntity, TEntityDto, TCreateUpdateEntityDto>
    {
        public Task<TEntityDto> FetchAsync(Guid id);

        public Task<TEntityDto> InsertAsync(TCreateUpdateEntityDto createUpdateEntityDto);

        public Task<TEntityDto> RefreshAsync(TCreateUpdateEntityDto createUpdateEntityDto);

        public Task<List<TEntityDto>> FetchManyAsync();

        public Task<bool> RemoveAsync(Guid id);

        void AddMany(List<TEntity> entities);

        public Task<bool> DeleteAsync(TEntity entity);

        public Task<bool> DeleteAsync(Guid id);

        public Task<TEntity> AddAsync(TEntity entity);

        public Task<TEntity> GetAsync(Guid id);

        public Task<TEntity> UpdateAsync(TEntity fromObj);
    }
}