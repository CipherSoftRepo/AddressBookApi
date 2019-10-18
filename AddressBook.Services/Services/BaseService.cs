using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBook.Data.Entities;
using AddressBook.Data.Interfaces;
using AddressBook.Services.BMOModels;
using AddressBook.Services.DTOModels;
using AddressBook.Services.Extensions;
using AddressBook.Services.Interfaces;
using AddressBook.Services.Models;
using AddressBook.Services.Services.Custom;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Services.Services
{
    public abstract class GroundBaseService
    {
        // just because easier config for autofac registration of services
    }

    public abstract class BaseService<TEntity, TDTOModel, TDTOListModel, TBMOModel> : GroundBaseService, IService<TDTOModel, TDTOListModel, TBMOModel>
        where TEntity : EntityBase, IDeletableEntity, IIdEntity, new()
        where TDTOModel : ModelBaseDTO
        where TDTOListModel : ModelBaseDTO
        where TBMOModel : ModelBaseBMO
    {
        public System.Security.Claims.ClaimsIdentity CurrentUser { get; set; }
        // public ClaimsPrincipal CurrentUser { get; set; }

        protected IGenericRepository<TEntity> EntityRepository { get; } 

        public BaseService(IGenericRepository<TEntity> entityRepository)
        {
            EntityRepository = entityRepository; 
        }


        #region IService
     

        public async virtual Task<PaginationList<IEnumerable<TDTOListModel>>> GetAllAsync(PaginationParameters paginateConfig = null)
        {
            var query = GetAllEntities().AsNoTracking();

            if (paginateConfig != null && paginateConfig.Query != null)
                foreach (var queryModel in paginateConfig.Query)
                {
                    query = GetSearchQuery(query, queryModel.Property, typeof(TEntity).Name, queryModel.Value);
                }


            var total = await query.CountAsync();

            query = query.Paginate(paginateConfig, typeof(TDTOListModel).Name);

            var entities = await query.ToListAsync().ConfigureAwait(false);
            var result = Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDTOListModel>>(entities);

            return new PaginationList<IEnumerable<TDTOListModel>>(result, total);
        }

        public async virtual Task<TDTOModel> GetAsync(Guid id)
        {
            var entity = await GetEntityByIdAsync(GetAllEntities().AsNoTracking(), id).ConfigureAwait(false);

            return Mapper.Map<TDTOModel>(entity);
        }

        public async virtual Task<TDTOModel> CreateAsync(TBMOModel model)
        {
            model.RejectIfInvalid();

            var entity = new TEntity();
            await UpdateEntity(entity, model, true);

            EntityRepository.Add(entity);
            await EntityRepository.SaveAsync().ConfigureAwait(false);

            entity = await GetEntityByIdAsync(GetAllEntities().AsNoTracking(), entity.Id).ConfigureAwait(false);

            return Mapper.Map<TDTOModel>(entity);
        }

        public async virtual Task<IEnumerable<TDTOListModel>> CreateBulkAsync(IEnumerable<TBMOModel> models)
        {
            models.RejectIfInvalid();

            var entities = new List<TEntity>();
            try
            {
                foreach (var model in models)
                {
                    var entity = new TEntity();
                    await UpdateEntity(entity, model, true);

                    EntityRepository.Add(entity);
                    await EntityRepository.SaveAsync().ConfigureAwait(false);

                    entity = await GetEntityByIdAsync(GetAllEntities(), entity.Id).ConfigureAwait(false);

                    entities.Add(entity);
                }
            }
            catch (Exception ex)
            {
                // TODO log it
            }

            // return ones that got created
            return Mapper.Map<IEnumerable<TDTOListModel>>(entities);
        }

        public async virtual Task<TDTOModel> UpdateAsync(Guid id, TBMOModel model)
        {
            model.RejectIfInvalid();

            var entity = await GetEntityByIdAsync(GetAllEntities(), id).ConfigureAwait(false);
            entity.RejectIfNotFound();

            await UpdateEntity(entity, model);

            await EntityRepository.SaveAsync().ConfigureAwait(false);

            entity = await GetEntityByIdAsync(GetAllEntities().AsNoTracking(), id).ConfigureAwait(false);

            return Mapper.Map<TDTOModel>(entity);
        }

        public async virtual Task DeleteAsync(Guid id)
        {
            var entity = await GetEntityByIdAsync(GetAllEntities(), id).ConfigureAwait(false);

            if (null == entity) return; // no need to delete anything

            // to really delete uncomment it
            // _entityRepository.Delete(entity);

            // is been used for soft delete!
            entity.HasBeenDeleted = DateTime.UtcNow;

            await EntityRepository.SaveAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// peramnent delete from db
        /// </summary>
        /// <param name="id">id of entity to be deleted</param>
        /// <returns></returns>
        public async virtual Task DeletePermanentlyAsync(Guid id)
        {
            var entity = await GetEntityByIdAsync(GetAllEntities(), id).ConfigureAwait(false);

            if (null == entity) return; // no need to delete anything

            // really delete
            EntityRepository.Delete(entity);

            await EntityRepository.SaveAsync().ConfigureAwait(false);
        }

        #endregion IService

        public IQueryable<TEntity> GetSearchQuery(IQueryable<TEntity> query, string propertyName, string mappedTotypeName, string queryString)
        {

            return SearchFilterInSqlService<TEntity>.GetSearchQuery(query, propertyName, mappedTotypeName, queryString);
        }

        protected virtual IQueryable<TEntity> GetAllEntities()
        {
            var query = EntityRepository.GetAll().FilterOutDeleted();
        

            return query;
        }

        protected virtual async Task<TEntity> GetEntityByIdAsync(IQueryable<TEntity> query, Guid id)
        {
            // var a = query.ToList();
            var entity = await query.SingleOrDefaultAsync(_ => _.Id == id).ConfigureAwait(false);
            return entity;
        }

        protected async virtual Task UpdateEntity(TEntity entity, TBMOModel updateModel, bool created = false)
        {
            Mapper.Map<TBMOModel, TEntity>(updateModel, entity);

            var dateTime = DateTime.UtcNow;

            if (created)
            {
                await UpdateICreatedProperties(entity, dateTime);
            }

            await UpdateIModifiedProperties(entity, dateTime);
        }

       

        protected virtual async Task UpdateICreatedProperties(object entity, DateTime createdTime)
        {
            if (null == entity) return;

           

            // check base object
            if (typeof(ICreatedEntity).IsAssignableFrom(entity.GetType()))
            {
                ((ICreatedEntity)entity).Created = createdTime;
             
            }

            var iCreatedProperties = entity.GetType().GetProperties().Where(p => typeof(ICreatedEntity).IsAssignableFrom(p.PropertyType)
                                                                                || typeof(IEnumerable<ICreatedEntity>).IsAssignableFrom(p.PropertyType));
            if (iCreatedProperties.Count() == 0) return;

            foreach (var prop in iCreatedProperties)
            {
                if (typeof(IEnumerable<ICreatedEntity>).IsAssignableFrom(prop.PropertyType))
                {
                    var propList = (IEnumerable<ICreatedEntity>)entity.GetType().GetProperty(prop.Name).GetValue(entity);

                    if (null == propList) continue;

                    foreach (var p in propList)
                    {
                        UpdateICreatedProperties(p, createdTime);
                    }
                }
                else if (typeof(ICreatedEntity).IsAssignableFrom(prop.PropertyType))
                {
                    var propOne = entity.GetType().GetProperty(prop.Name).GetValue(entity);

                    if (null == propOne) continue;

                    UpdateICreatedProperties(propOne, createdTime);
                }
            }
        }

        protected virtual async Task UpdateIModifiedProperties(object entity, DateTime modifiedTime)
        {
            if (null == entity) return;

        
            // check base object
            if (typeof(IModifiedEntity).IsAssignableFrom(entity.GetType()))
            {
                ((IModifiedEntity)entity).Modified = modifiedTime;
            }

        
        }
    }
}
