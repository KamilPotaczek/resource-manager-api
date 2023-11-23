namespace ResourceManager.Contracts.Users;

public record UserResponse(Guid Id, string Email, UserRole Role);