namespace ResourceManager.Contracts.Resources;

public record ResourceResponse(Guid Id, bool IsWithdrawn, bool IsLocked);