namespace ResourceManager.Contracts.Users;

public record AddUserRequest(string Email, UserRole Role);