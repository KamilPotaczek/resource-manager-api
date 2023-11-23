using System.Text.Json.Serialization;

namespace ResourceManager.Contracts.Users;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Admin,
    User
}