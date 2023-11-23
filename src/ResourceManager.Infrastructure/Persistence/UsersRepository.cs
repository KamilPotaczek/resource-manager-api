using ResourceManager.Application.Users;
using ResourceManager.Domain.Users;

namespace ResourceManager.Infrastructure.Persistence;

internal sealed class UsersRepository : IUsersRepository
{
    private readonly List<User> _users = new();

    public Task<List<User>> ListAsync()
    {
        return Task.FromResult(_users.ToList());
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        var user = _users.Find(x => x.Id == id);
        return Task.FromResult(user);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        var user = _users.Find(x => x.Email == email);
        return Task.FromResult(user);
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        var result = _users.Exists(user => user.Id == id);
        return Task.FromResult(result);
    }

    public Task<bool> ExistsAsync(string email)
    {
        var result = _users.Exists(user => user.Email == email);
        return Task.FromResult(result);    }

    public Task RemoveAsync(User user)
    {
        _users.Remove(user);
        return Task.CompletedTask;
    }

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}