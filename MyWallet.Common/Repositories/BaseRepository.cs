using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWallet.Common.Interfaces;
using MyWallet.Common.Models;

namespace MyWallet.Common.Repositories;

public class BaseRepository<TContext, TDto, TEntity>(TContext context, IMapper mapper) : IBaseRepository<TDto> 
    where TContext : DbContext 
    where TEntity : BaseEntity, new()
    where TDto : BaseEntityDto, new()
{

    public async virtual Task<TDto> GetAsync(int id)
    {
        var entity = await context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return mapper.Map<TDto>(entity);
    }

    public async virtual Task AddAsync(TDto dto)
    {
        var entity = mapper.Map<TEntity>(dto);
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id;
    }

    public virtual void Update(TDto dto)
    {
        var entity = mapper.Map<TEntity>(dto);
        entity.DateModified = DateTime.UtcNow;
        context.Update(entity);
    }
    public virtual void Delete(int id)
    {
        context.Set<TEntity>().Where(x => x.Id == id)
            .ExecuteUpdate(entity => 
            entity.SetProperty(x => x.IsSoftDeleted, true)
                    .SetProperty(x => x.DateModified, DateTime.UtcNow));
    }

    public async virtual Task<bool> ExistsAsync(int id)
    {
        return await context.Set<TEntity>()
            .AnyAsync(x => x.Id == id);
    } 

    public virtual async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
