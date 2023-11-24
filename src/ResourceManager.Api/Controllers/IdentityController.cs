using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResourceManager.Application.Users;
using ResourceManager.Contracts.Identity;

namespace ResourceManager.Api.Controllers;


[ApiController]
[Route("[controller]")]
public sealed class IdentityController : ControllerBase
{
    private const string TokenSecret = "ExtremelyPrivateTokenStoredInSafeStorageInProduction";
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);
    private readonly IUsersRepository _usersRepository;

    public IdentityController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
    }

    /// <summary>
    /// Use result of this endpoint for authenticating the rest of the calls.
    /// </summary>
    /// <remarks> In a real life scenario this endpoint would be a separate IdentityService </remarks>
    /// <returns> A JWT token with auth information. </returns>
    [HttpPost("token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest query)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);
        var user = await _usersRepository.GetByIdAsync(query.UserId);

        if (user is null) return Unauthorized("There is no user with such ID and email");
        
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("userId", query.UserId.ToString()),
            new("userRole", user.Role.Name),
            new("userEmail", user.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifetime),
            Issuer = "https://id.resourcemanager.com",
            Audience = "https://api.resourcemanager.com",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwt = tokenHandler.WriteToken(token);
        return Ok(jwt);
    }
}