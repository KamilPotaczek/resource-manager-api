using Microsoft.EntityFrameworkCore;
using ResourceManager.Application.Resources;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Infrastructure.Persistence.Repositories;

internal sealed class ResourcesRepository : IResourcesRepository
{
    private readonly ResourceManagerDbContext _dbContext; 

    public ResourcesRepository(ResourceManagerDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<List<Resource>> ListAsync()
    {
        return await _dbContext.Resources.ToListAsync();
    }

    public async Task<Resource?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Resources
            .Include(res => res.ResourceLock)
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Resources
            .AsNoTracking()
            .AnyAsync(user => user.Id == id);
    }

    public Task RemoveAsync(Resource resource)
    {
        _dbContext.Remove(resource);
        return Task.CompletedTask;
    }

    public async Task AddAsync(Resource resource)
    {
        await _dbContext.Resources.AddAsync(resource);
    }

    public async Task UpdateAsync(Resource resource)
    {
        _dbContext.Update(resource);

        if (resource.ResourceLock == null)
        {
            var resourceLock = await _dbContext.ResourceLocks.FirstOrDefaultAsync(rl => rl.ResourceId == resource.Id);
            if (resourceLock is not null)
                _dbContext.Remove(resourceLock);
            return;
        }

        await UpsertAsync(resource.ResourceLock);
    }
    
    private async Task UpsertAsync(ResourceLock resourceLock)
    {
        var existingLock = await _dbContext.ResourceLocks
            .AsNoTracking()
            .FirstOrDefaultAsync(rl => rl.ResourceId == resourceLock.ResourceId);
        if (existingLock is not null)
        {
            _dbContext.ResourceLocks.Update(resourceLock);
            return;
        }

        await _dbContext.ResourceLocks.AddAsync(resourceLock);
    }
    
}