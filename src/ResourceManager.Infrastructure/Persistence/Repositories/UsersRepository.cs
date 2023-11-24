using System.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ResourceManager.Application.Users;
using ResourceManager.Domain.Users;

namespace ResourceManager.Infrastructure.Persistence.Repositories;

internal sealed class UsersRepository : IUsersRepository
{
    private readonly ResourceManagerDbContext _dbContext;

    public UsersRepository(ResourceManagerDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<List<User>> ListAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Id == id);
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == email);
    }

    public Task RemoveAsync(User user)
    {
        _dbContext.Users.Remove(user);
        return Task.CompletedTask;
    }

    public Task AddAsync(User user)
    {
        _dbContext.Users.Add(user);
        return Task.CompletedTask;
    }
    
    public async Task<ErrorOr<Success>> CommitChangesAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            return Result.Success;
        }
        catch (Exception e)
        {
            return e switch
            {
                DBConcurrencyException concurrencyException => Error.Conflict(description: concurrencyException.Message),
                _ => Error.Failure(description: e.Message)
            };
        }
    }
}