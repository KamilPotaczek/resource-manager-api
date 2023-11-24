namespace ResourceManager.Application.Common;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}

public sealed class DateTimeProvider : IDateTimeProvider
{
    DateTime IDateTimeProvider.UtcNow => DateTime.UtcNow;
}