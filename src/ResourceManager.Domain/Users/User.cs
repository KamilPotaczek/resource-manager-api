namespace ResourceManager.Domain.Users;

public sealed class User
{
    public Guid Id { get; private set; }
    public UserRole Role { get; private set; }
    public string Email { get; private set; }

    public User(string email, UserRole? role = null, Guid? id = null)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));

        Id = id ?? Guid.NewGuid();
        Role = role ?? UserRole.User;
    }
}