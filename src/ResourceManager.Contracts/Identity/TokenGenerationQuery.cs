namespace ResourceManager.Contracts.Identity;

public record TokenGenerationQuery(Guid UserId, string Email);