using ResourceManager.Domain.Users;

namespace ResourceManager.Application.Users;

public interface IUsersRepository
{
    Task<List<User>> ListAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(string email);
    Task RemoveAsync(User user);
    Task AddAsync(User user);
}