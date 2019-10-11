namespace LafAPI.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using LafAPI.Data.Common.Models;
    using LafAPI.Data.Common.Repositories;

    using Microsoft.EntityFrameworkCore;

    public class EfDeletableEntityRepository<TEntity> : EfRepository<TEntity>, IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        public EfDeletableEntityRepository(LafContext context) : base(context)
        {
        }

        public override IQueryable<TEntity> All() => base.All().Where(x => !x.IsDeleted);

        public override IQueryable<TEntity> AllAsNoTracking() => base.AllAsNoTracking().Where(x => !x.IsDeleted);

        public IQueryable<TEntity> AllWithDeleted() => base.All().IgnoreQueryFilters();

        public IQueryable<TEntity> AllAsNoTrackingWithDeleted() => base.AllAsNoTracking().IgnoreQueryFilters();

        public override async Task<TEntity> GetByIdAsync(params object[] id)
        {
            var entity = await base.GetByIdAsync(id);
            if (entity?.IsDeleted ?? false)
            {
                entity = null;
            }

            return entity;
        }

        public Task<TEntity> GetByIdWithDeletedAsync(params object[] id) =>
            this.AllWithDeleted().FirstOrDefaultAsync(
                EfExpressionHelper.BuildByIdPredicate<TEntity>(this.Context, id));

        public void HardDelete(TEntity entity) =>
            base.Delete(entity);

        public void Undelete(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.Update(entity);
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            this.Update(entity);
        }
    }
}