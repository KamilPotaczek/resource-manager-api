using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResourceManager.Contracts.Identity;

namespace ResourceManager.Api.Controllers;


[ApiController]
[Route("[controller]")]
public sealed class IdentityController : ControllerBase
{
    private const string TokenSecret = "ExtremelyPrivateTokenStoredInSafeStorageInProduction";
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);

    /// <summary>
    /// Use result of this endpoint for authenticating the rest of the calls.
    /// </summary>
    /// <remarks> In a real life scenario this endpoint would be a separate IdentityService </remarks>
    /// <returns> A JWT token with auth information. </returns>
    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] TokenGenerationQuery query)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, query.Email),
            new(JwtRegisteredClaimNames.Email, query.Email),
            new("userid", query.UserId.ToString())
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