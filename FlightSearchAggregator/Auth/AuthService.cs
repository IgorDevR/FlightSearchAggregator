using FlightSearchAggregator.DbContext;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FlightSearchAggregator.Auth;

public class AuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool Register(string fullName, string email, string password)
    {
        if (DbEmulation.Users.ContainsKey(email))
        {
            return false;
        }

        var passwordHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes(password)));

        var user = new User { FullName = fullName, Email = email, PasswordHash = passwordHash };
        DbEmulation.Users.Add(email, user);

        return true;
    }

    public string Login(string email, string password)
    {
        if (!DbEmulation.Users.ContainsKey(email))
        {
            return null;
        }

        var user = DbEmulation.Users[email];

        var passwordHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes(password)));
        if (user.PasswordHash != passwordHash)
        {
            return null;
        }

        return GenerateJwtToken(email);
    }

    private string GenerateJwtToken(string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}