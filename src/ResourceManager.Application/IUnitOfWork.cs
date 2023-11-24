namespace ResourceManager.Application;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}