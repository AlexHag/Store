using server.Models;
using server.Context;
using server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace server.Controllers;

[ApiController]
[Route("api")]
public class UserAuthenticationController : ControllerBase
{
    private DataContext _context;
    private readonly IConfiguration _config;
    private readonly IHelperFunctions _helper;

    public UserAuthenticationController(DataContext context, IConfiguration config, IHelperFunctions helper)
    {
        _context = context;
        _config = config;
        _helper = helper;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO UserRegisterRequest)
    {
        if(UserRegisterRequest.Role.ToLower() != "user" && UserRegisterRequest.Role.ToLower() != "storeowner"){
            return BadRequest("Can only register as User or Store owner");
        }

        // TODO: Add email regex verification

        var existingUser = _context.Users
            .FirstOrDefault(u => u.Email == UserRegisterRequest.Email);

        if (existingUser != null) return BadRequest("Username already exists");

        var userSalt = _helper.RandomString(16);
        var passwordHash = _helper.HashString(UserRegisterRequest.Password + userSalt);

        await _context.Users.AddAsync(new User 
        {
            Id = Guid.NewGuid(),
            Email = UserRegisterRequest.Email,
            Password = passwordHash,
            Salt = userSalt,
            Role = UserRegisterRequest.Role,
            StoreId = UserRegisterRequest.Role.ToLower() == "user" ? Guid.Empty : Guid.NewGuid()
        });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] UserLoginDTO UserLoginRequest)
    {
        var userSalt = _context.Users.Where(u => u.Email == UserLoginRequest.Email).Select(u => u.Salt).FirstOrDefault();
        if (userSalt == null) return BadRequest("Wrong username or password");
        var passwordHash = _helper.HashString(UserLoginRequest.Password + userSalt);
        
        var existingUser = _context.Users
            .Where(u => u.Email == UserLoginRequest.Email && u.Password == passwordHash).FirstOrDefault();

        if (existingUser == null) return BadRequest("Wrong username or password");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Id", existingUser.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}