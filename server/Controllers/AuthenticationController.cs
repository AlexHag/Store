using server.Models;
using server.Context;
using server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

        var newUser = _helper.CreateNewUserObject(UserRegisterRequest);
        await _context.Users.AddAsync(newUser);

        if(UserRegisterRequest.Role == "storeowner") {
            if(String.IsNullOrEmpty(UserRegisterRequest.StoreName)) return BadRequest("Store owner must provide a store name");
            await _context.Stores.AddAsync(new Store {
                Id = newUser.StoreId,
                Name = UserRegisterRequest.StoreName,
                StoreOwnerId = newUser.Id
            });
        }
        await _context.SaveChangesAsync();

        var token = _helper.CreateJWT(newUser.Id);

        return Ok(new { token = token });
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

        var token = _helper.CreateJWT(existingUser.Id);

        return Ok(new { token = token });
    }

    [Authorize]
    [HttpGet]
    [Route("userinfo")]
    public IActionResult GetUserInfo()
    {
        var userId = _helper.GetRequestUserId(HttpContext);
        var user = _context.Users.Find(userId);
        if (user == null) return BadRequest("User not found from JWT claim. This should not be possible.");

        var userStore = _context.Stores.Where(p => p.StoreOwnerId == user.Id).FirstOrDefault();
        return Ok(new UserInfoDTO{ 
            Email = user.Email,
            Role = user.Role,
            StoreName = userStore == null ? "" : userStore.Name
        });
    }
}