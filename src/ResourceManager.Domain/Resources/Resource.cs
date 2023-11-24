using ErrorOr;
using ResourceManager.Domain.Users;

namespace ResourceManager.Domain.Resources;

public class Resource
{
    public Resource(Guid? id)
    {
        Id = id ?? Guid.NewGuid();
        IsWithdrawn = false;
    }

    private Resource()
    {
    }

    public Guid Id { get; private set; }
    public bool IsWithdrawn { get; private set; }
    public ResourceLock? ResourceLock { get; private set; }

    public ErrorOr<Success> Lock(User user, DateTime? until, DateTime now)
    {
        if (IsWithdrawn)
            return ResourceErrors.CannotLockWithdrawnResource;

        if (IsLockedByAnotherUser(user, now))
            return ResourceErrors.CannotLockResourceLockedByAnotherUser;

        if (now > until)
            return ResourceErrors.CannotLockResourceWithPastEndDate;

        UpsertLock(user, until);

        return Result.Success;
    }

    private void UpsertLock(User user, DateTime? until)
    {
        if (ResourceLock is not null)
            ResourceLock.UpdateLockValidity(until);
        else
            ResourceLock = new ResourceLock(until, user, this);
    }


    public ErrorOr<Success> Unlock(User user, DateTime now)
    {
        if (IsLockedByAnotherUser(user, now))
            return ResourceErrors.CannotUnlockResourceLockedByAnotherUser;

        ResourceLock = null;
        return Result.Success;
    }

    public bool IsLocked(DateTime now)
    {
        if (ResourceLock == null) return false;

        return ResourceLock.Until >= now || ResourceLock.IsUfnLocked;
    }

    public ErrorOr<Success> Withdraw(User user)
    {
        if (user.Role != UserRole.Admin)
            return Error.Unauthorized(description: "Only Admin user can withdraw resource");
        IsWithdrawn = true;
        ResourceLock = null;
        return Result.Success;
    }

    private bool IsLockedByAnotherUser(User user, DateTime now)
    {
        return IsLocked(now) && user.Id != ResourceLock!.UserId;
    }
}