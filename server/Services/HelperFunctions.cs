using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using server.Models;

namespace server.Services;

public class HelperFunctions : IHelperFunctions
{
    private static Random random = new Random();
    private readonly IConfiguration _config;

    public HelperFunctions(IConfiguration config) 
    {
         _config = config;
    }

    public string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public string HashString(string input)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())            
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(input));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    public Guid GetRequestUserId(HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        if(identity == null)
        {
            throw new Exception("Identity is null. This should not happen and be handeled by the [Authorize] attribute.");
        }

        IEnumerable<Claim> claims = identity.Claims; 
        var claimId = identity.FindFirst("Id")?.Value;
        
        if(claimId == null)
        {
            throw new Exception("Identity is null. This should not happen and be handeled by the [Authorize] attribute.");
        }

        return Guid.Parse(claimId);
    }

    public string CreateJWT(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Id", userId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public User CreateNewUserObject(UserRegisterDTO UserRegisterRequest)
    {
        var userSalt = RandomString(16);
        var passwordHash = HashString(UserRegisterRequest.Password + userSalt);

        return new User{
            Id = Guid.NewGuid(),
            Email = UserRegisterRequest.Email,
            Password = passwordHash,
            Salt = userSalt,
            Role = UserRegisterRequest.Role,
            StoreId = UserRegisterRequest.Role.ToLower() == "user" ? Guid.Empty : Guid.NewGuid()
        };
    }
}