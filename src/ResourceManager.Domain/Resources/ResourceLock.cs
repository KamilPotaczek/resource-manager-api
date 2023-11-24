using ResourceManager.Domain.Users;

namespace ResourceManager.Domain.Resources;

public sealed class ResourceLock
{
    public ResourceLock(DateTime? until, User user, Resource resource, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Until = until;
        UserId = user.Id;
        ResourceId = resource.Id;
    }

    internal void UpdateLockValidity(DateTime? until)
    {
        Until = until;
    }
    
    private ResourceLock()
    {
    }

    public Guid Id { get; set; }
    public DateTime? Until { get; private set; }
    public bool IsUfnLocked => !Until.HasValue;
    public Guid UserId { get; private set; }
    public Guid ResourceId { get; private set; }
}