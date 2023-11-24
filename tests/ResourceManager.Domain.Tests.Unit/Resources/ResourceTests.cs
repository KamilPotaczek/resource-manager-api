using ResourceManager.Domain.Resources;
using ResourceManager.Domain.Users;

namespace ResourceManager.Domain.Tests.Unit.Resources;


public sealed class ResourceTests
{
    private readonly Guid _resourceId = Guid.Parse("FE51C8A1-C8FD-44AA-B8D1-B98781849909");
    private readonly DateTime _currentTime = new(2023, 10, 10);
    private const string Email = "testuser@resourcemanager.io";

    [Fact]
    public void Creation_WhenResourceIsCreated_ThenItIsNotWithdrawn()
    {
        var sut = new Resource(_resourceId);
        sut.IsWithdrawn.Should().BeFalse();
        sut.Id.Should().Be(_resourceId);
    }
    
    [Fact]
    public void Withdraw_WhenNonAdminUserWithdrawsResource_ThenUnauthorizedIsReturned()
    {
        var sut = new Resource(_resourceId);
        var user = new User(Email, UserRole.User);

        var result = sut.Withdraw(user);
        
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        sut.IsWithdrawn.Should().BeFalse();
    }

    [Fact]
    public void Withdraw_WhenAdminUserWithdrawsResource_ThenResourceIsWithdrawn()
    {
        var sut = new Resource(_resourceId);
        var user = new User(Email, UserRole.Admin);

        var result = sut.Withdraw(user);
        
        result.Value.Should().Be(Result.Success);
        sut.IsWithdrawn.Should().BeTrue();
    }
    
    [Fact]
    public void Withdraw_WhenResourceIsWithdrawn_ThenLockIsRemoved()
    {
        var sut = new Resource(_resourceId);
        var user = new User(Email, UserRole.Admin);

        var result = sut.Withdraw(user);
        
        result.Value.Should().Be(Result.Success);
        sut.IsWithdrawn.Should().BeTrue();
        sut.ResourceLock.Should().BeNull();
    }

    [Fact]
    public void Lock_WhenResourceIsWithdrawn_ThenItCannotBeLocked()
    {
        var sut = new Resource(_resourceId);
        var user = new User(Email, UserRole.Admin);
        sut.Withdraw(user);

        var result = sut.Lock(user, null, _currentTime);
        
        result.FirstError.Should().Be(ResourceErrors.CannotLockWithdrawnResource);
    }
    
    [Fact]
    public void Lock_WhenLockingUnlockedResource_ThenResourceIsLocked()
    {
        var sut = new Resource(_resourceId);
        var user = new User(Email, UserRole.Admin);

        var result = sut.Lock(user, null, _currentTime);

        result.IsError.Should().BeFalse();
        sut.IsLocked(_currentTime).Should().BeTrue();
    }
    
    [Fact]
    public void Lock_WhenLockedForAWeek_ThenResourceIsNotLockedAfterwards()
    {
        var sut = new Resource(_resourceId);
        var user = new User(Email, UserRole.Admin);

        var result = sut.Lock(user, _currentTime.AddDays(7), _currentTime);

        result.IsError.Should().BeFalse();
        sut.IsLocked(_currentTime.AddDays(6)).Should().BeTrue();
        sut.IsLocked(_currentTime.AddDays(8)).Should().BeFalse();
    }
    
    [Fact]
    public void Lock_WhenLockedWithPastDate_ThenErrorIsReturnedAndResourceIsNotLocked()
    {
        var sut = new Resource(_resourceId);
        var user = new User(Email, UserRole.Admin);

        var result = sut.Lock(user, _currentTime.AddDays(-1), _currentTime);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(ResourceErrors.CannotLockResourceWithPastEndDate);
        sut.IsLocked(_currentTime).Should().BeFalse();
    }

    [Fact]
    public void Unlock_WhenUnlockingOtherUserResource_ThenErrorIsReturnedAndResourceIsNotUnlocked()
    {
        var sut = new Resource(_resourceId);
        var originalLocker = new User(Email, UserRole.Admin);
        sut.Lock(originalLocker, null, _currentTime);
        var anotherUser = new User("anotherUser", UserRole.User);

        var result = sut.Unlock(anotherUser, _currentTime);

        result.FirstError.Should().Be(ResourceErrors.CannotUnlockResourceLockedByAnotherUser);
        sut.IsLocked(_currentTime).Should().BeTrue();
    }
    
    
    [Fact]
    public void Unlock_WhenUnlockingOwnResource_ThenResourceIsUnlocked()
    {
        var sut = new Resource(_resourceId);
        var originalLocker = new User(Email, UserRole.Admin);
        sut.Lock(originalLocker, null, _currentTime);

        var result = sut.Unlock(originalLocker, _currentTime);

        result.IsError.Should().BeFalse();
        sut.IsLocked(_currentTime).Should().BeFalse();
    }
}