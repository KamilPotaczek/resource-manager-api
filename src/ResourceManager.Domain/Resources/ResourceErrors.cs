using ErrorOr;

namespace ResourceManager.Domain.Resources;

public static class ResourceErrors
{
    public static readonly Error CannotLockWithdrawnResource = Error.Validation(
        "Resource.CannotLockWithdrawnResource",
        "Cannot lock a resource that has been withdrawn");

    public static readonly Error CannotLockResourceLockedByAnotherUser = Error.Validation(
        "Resource.CannotLockResourceLockedByAnotherUser",
        "Cannot lock a resource locked by another user");

    public static readonly Error CannotUnlockResourceLockedByAnotherUser = Error.Validation(
        "Resource.CannotUnlockResourceLockedByAnotherUser",
        "Cannot unlock a resource locked by another user");
    
    public static readonly Error CannotLockResourceWithPastEndDate = Error.Validation(
        "Resource.CannotLockResourceWithPastEndDate",
        "Cannot lock a resource with past end date");
}