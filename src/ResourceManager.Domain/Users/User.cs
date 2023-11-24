namespace ResourceManager.Domain.Users;

public sealed class User
{
    public User(string email, UserRole? role = null, Guid? id = null)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));

        Id = id ?? Guid.NewGuid();
        Role = role ?? UserRole.User;
    }

    private User()
    {
    }

    public Guid Id { get; private set; }
    public UserRole Role { get; private set; } = null!;
    public string Email { get; private set; } = null!;
}