namespace ResourceManager.Contracts.Identity;

public record GenerateTokenRequest(Guid UserId, string Email);