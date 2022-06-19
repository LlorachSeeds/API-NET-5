using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Services.UTLs;

namespace Services.REPs
{
    public class
        ServiceRepository<TEntity, TEntityDto, TCreateUpdateEntityDto> : IServiceRepository<TEntity, TEntityDto,
            TCreateUpdateEntityDto>
        where TEntity : class
    {
        protected readonly IConfiguration Configuration;

        internal Context Context;

        public DbSet<TEntity> DbSet;

        protected string ChirpstackApiRoute { get; set; }

        public ServiceRepository(Context context, IConfiguration configuration)
        {
            Configuration = configuration;
            Context = context;
            DbSet = context.Set<TEntity>();
            ChirpstackApiRoute = Configuration["ChirpstackApiRoute"];
        }

        public virtual async Task<TEntityDto> FetchAsync(Guid id)
        {
            TEntityDto entityDto = default(TEntityDto);

            if (id != Guid.Empty)
            {
                TEntity entity = await GetAsync(id);
                if (entity != null)
                {
                    entityDto = Utilities.Mapper.Map<TEntityDto>(entity);
                }
                else
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, "FetchAsync", Errors.ElementNotFound);
                }
            }

            return entityDto;
        }

        public virtual async Task<TEntityDto> InsertAsync(TCreateUpdateEntityDto createUpdateEntityDto)
        {
            TEntityDto entityDto = default(TEntityDto);

            if (createUpdateEntityDto != null)
            {
                TEntity entityToAdd = Utilities.Mapper.Map<TEntity>(createUpdateEntityDto);
                if (entityToAdd != null)
                {
                    var entity = await AddAsync(entityToAdd);
                    if (entity != null)
                    {
                        entityDto = Utilities.Mapper.Map<TEntityDto>(entity);
                    }
                }
            }

            return entityDto;
        }

        public virtual async Task<TEntityDto> RefreshAsync(TCreateUpdateEntityDto createUpdateEntityDto)
        {
            TEntityDto entityDto = default(TEntityDto);

            if (createUpdateEntityDto != null)
            {
                TEntity entity = Utilities.Mapper.Map<TEntity>(createUpdateEntityDto);
                if (entity != null)
                {
                    TEntity updatedEntity = await UpdateAsync(entity);
                    if (updatedEntity != null)
                    {
                        entityDto = Utilities.Mapper.Map<TEntityDto>(updatedEntity);
                    }
                }
            }

            return entityDto;
        }

        public virtual async Task<List<TEntityDto>> FetchManyAsync()
        {
            List<TEntityDto> listEntityDto = null;
            List<TEntity> entities = await GetListAsync();
            if (entities != null)
            {
                listEntityDto = Utilities.Mapper.Map<List<TEntity>, List<TEntityDto>>(entities);
            }
            else
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "FetchManyAsync", Errors.ElementNotFound);
            }

            return listEntityDto;
        }

        public virtual async Task<bool> RemoveAsync(Guid id)
        {
            bool result = false;
            if (id != Guid.Empty)
            {
                result = await DeleteAsync(id);
            }

            return result;
        }

        // Repo Methods
        public virtual void AddMany(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            bool result = false;
            if (entity != null)
            {
                EntityEntry deletedEntity = DbSet.Remove(entity);
                if (deletedEntity.State == EntityState.Deleted)
                {
                    result = true;
                }
            }

            return result;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            bool result = false;
            if (id != Guid.Empty)
            {
                TEntity entity = await GetAsync(id);
                result = await DeleteAsync(entity);
            }

            return result;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            EntityEntry<TEntity> savedEntity = await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return savedEntity.Entity;
        }

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            TEntity element = null;
            if (id != Guid.Empty)
            {
                element = await DbSet.FindAsync(id);
            }

            return element;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity fromObj)
        {
            TEntity databaseEntity = null;
            if (fromObj != null)
            {
                databaseEntity = await DbSet.Where(e => e.Equals(fromObj)).FirstOrDefaultAsync();

                try
                {
                    object toEntity = (object)databaseEntity;
                    object fromEntity = (object)fromObj;
                    UpdateObject(ref toEntity, ref fromEntity);

                    databaseEntity = (TEntity)toEntity;

                    DbSet.Update(databaseEntity);
                    await Context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return databaseEntity;
        }

        public virtual async Task<TEntity> UpdateEntityAndSonsAsync(TEntity fromObj)
        {
            TEntity databaseEntity = null;
            if (fromObj != null)
            {
                databaseEntity = await DbSet.Where(e => e.Equals(fromObj)).FirstOrDefaultAsync();
                LoadCollectionsToEntity(ref databaseEntity);

                try
                {
                    object toEntity = (object)databaseEntity;
                    object fromEntity = (object)fromObj;
                    UpdateObject(ref toEntity, ref fromEntity);
                    foreach (var toProp in toEntity.GetType().GetProperties())
                    {
                        var fromProp = fromEntity.GetType().GetProperty(toProp.Name);

                        if (fromProp != null && fromProp.PropertyType.Namespace != "System")
                        {
                            var fromValue = fromProp.GetValue(fromEntity, null);
                            var toValue = toProp.GetValue(toEntity, null);

                            UpdateObject(ref toValue, ref fromValue);
                        }
                    }

                    databaseEntity = (TEntity)toEntity;

                    DbSet.Update(databaseEntity);
                    await Context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return databaseEntity;
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1508:AdvancedNamingRules")]
        private void UpdateObject(ref object toEntity, ref object fromEntity)
        {
            if (toEntity != null && fromEntity != null)
            {
                foreach (var toProp in toEntity.GetType().GetProperties())
                {
                    var fromProp = fromEntity.GetType().GetProperty(toProp.Name);
                    if (fromProp != null)
                    {
                        var toValue = toProp.GetValue(toEntity, null);
                        var fromValue = fromProp.GetValue(fromEntity, null);
                        if (fromValue != null && fromValue is not Guid && fromProp.PropertyType.Namespace == "System")
                        {
                            if (toValue is string)
                            {
                                if (!string.IsNullOrWhiteSpace(toValue.ToString()))
                                {
                                    toProp.SetValue(toEntity, fromValue, null);
                                }
                            }
                            else if (toValue is int)
                            {
                                if ((int)toValue != int.MinValue && (int)toValue != int.MaxValue)
                                {
                                    toProp.SetValue(toEntity, fromValue, null);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LoadCollectionsToEntity(ref TEntity entity)
        {
            foreach (var prop in typeof(TEntity).GetProperties())
            {
                var toValue = prop.GetValue(entity, null);
                if (prop.PropertyType.Namespace != "System")
                {
                    Context.Entry(entity).Reference(prop.Name).Load();
                }
            }
        }

        public virtual async Task<List<TEntity>> GetListAsync()
        {
            return await DbSet.ToListAsync();
        }
    }
}