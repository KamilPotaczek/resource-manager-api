using ResourceManager.Domain.Users;

namespace ResourceManager.Api;

public interface IUserContextProvider
{
    User GetUserFromContext();
}

internal sealed class UserContextProvider : IUserContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public User GetUserFromContext()
    {
        var claims = _httpContextAccessor.HttpContext!.User.Claims.ToList();
        var id = Guid.Parse(claims.First(claim => claim.Type == "userId").Value);
        var role = UserRole.FromName(claims.First(claim => claim.Type == "userRole").Value);
        var email = claims.First(claim => claim.Type == "userEmail").Value;
        return new User(email, role, id);
    }
}