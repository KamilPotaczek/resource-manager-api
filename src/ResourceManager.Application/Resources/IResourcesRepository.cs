using ErrorOr;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources;

public interface IResourcesRepository
{
    Task<List<Resource>> ListAsync();
    Task<Resource?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task RemoveAsync(Resource resource);
    Task AddAsync(Resource resource);
    Task UpdateAsync(Resource resource);
    Task<ErrorOr<Success>> CommitChangesAsync();
}