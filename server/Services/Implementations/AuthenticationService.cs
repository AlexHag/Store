

using server.Models;
using server.Context;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace server.Services;

public class AuthenticationService : IAuthenticationService
{
    private DataContext _context;
    private readonly IHelperFunctions _helper; 
    private readonly IConfiguration _config;

    public AuthenticationService(DataContext context, IConfiguration config, IHelperFunctions helper) 
    {
        _config = config;
        _context = context;
        _helper = helper;
    }

    public async Task<ControllerServiceResponse> RegisterUser(UserRegisterDTO UserRegisterRequest)
    {
        if(UserRegisterRequest.Role.ToLower() != "user" && UserRegisterRequest.Role.ToLower() != "storeowner") 
        {
            return new ControllerServiceResponse
            {
                IsSuccess = false,
                ErrorMessage = "Role must be either user or storeowner"
            };
        }

        var existingUser = _context.Users
            .FirstOrDefault(u => u.Email == UserRegisterRequest.Email);
        if (existingUser != null) 
        {
            return new ControllerServiceResponse
            {
                IsSuccess = false,
                ErrorMessage = "Email already exists"
            };
        }

        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var userSalt = new string(Enumerable.Repeat(chars, 16)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        
        var passwordHash = HashString(UserRegisterRequest.Password + userSalt);

        var newUser =  new User 
        {
            Id = Guid.NewGuid(),
            Email = UserRegisterRequest.Email,
            Password = passwordHash,
            Salt = userSalt,
            Role = UserRegisterRequest.Role,
            StoreId = UserRegisterRequest.Role.ToLower() == "user" ? Guid.Empty : Guid.NewGuid()
        };

        await _context.Users.AddAsync(newUser);

        if(UserRegisterRequest.Role == "storeowner") 
        {
            if(String.IsNullOrEmpty(UserRegisterRequest.StoreName)) 
            {
                return new ControllerServiceResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Store owner must provide a store name"
                };
            }
            await _context.Stores.AddAsync(new Store 
            {
                Id = newUser.StoreId,
                Name = UserRegisterRequest.StoreName,
                StoreOwnerId = newUser.Id
            });
        }

        await _context.SaveChangesAsync();
        var token = CreateJWT(newUser.Id);
        return new ControllerServiceResponse 
        {
            IsSuccess = true,
            Value = token
        };
    }

    public async Task<ControllerServiceResponse> LoginUser(UserLoginDTO UserLoginRequest)
    {
        var userSalt = _context.Users
            .Where(u => u.Email == UserLoginRequest.Email)
            .Select(u => u.Salt)
            .FirstOrDefault();
        if (userSalt == null) 
        {
            return new ControllerServiceResponse 
            {
                IsSuccess = false,
                ErrorMessage = "Wrong username or password"
            };
        }
        var passwordHash = HashString(UserLoginRequest.Password + userSalt);
        var existingUser = _context.Users
            .Where(u => u.Email == UserLoginRequest.Email && u.Password == passwordHash)
            .FirstOrDefault();
        if (existingUser == null) 
        {
            return new ControllerServiceResponse 
            {
                IsSuccess = false,
                ErrorMessage = "Wrong username or password"
            };
        }

        var token = CreateJWT(existingUser.Id);

        return new ControllerServiceResponse
        {
            IsSuccess = true,
            Value = token
        };
    }

    public async Task<ControllerServiceResponse> GetUserInfo(HttpContext httpContext) 
    {
        var userId = _helper.GetRequestUserId(httpContext);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("User is null eventhough jwt token is valid. This should not happen.");

        var userStore = _context.Stores.Where(p => p.StoreOwnerId == user.Id).FirstOrDefault();
        return new ControllerServiceResponse
        {
            IsSuccess = true,
            Value =  new UserInfoDTO { 
            Email = user.Email,
            Role = user.Role,
            StoreName = userStore == null ? "" : userStore.Name
            }
        };
    }
    
    private string CreateJWT(Guid userId)
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

    private string HashString(string input)
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
}