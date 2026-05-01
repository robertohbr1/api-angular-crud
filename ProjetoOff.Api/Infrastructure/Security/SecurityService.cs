using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoOff.Api.Infrastructure.Security;

public interface ISecurityService
{
    string GenerateJwtToken(string username);
}

public class SecurityService : ISecurityService
{
    private readonly IConfiguration _config;

    public SecurityService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateJwtToken(string username)
    {
        var secret = _config["Jwt:Secret"] ?? "a-very-long-and-secure-secret-key-1234567890";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"] ?? "ProjetoOff",
            audience: _config["Jwt:Audience"] ?? "ProjetoOff",
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
