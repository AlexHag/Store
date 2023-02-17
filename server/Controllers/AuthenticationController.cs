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
    private readonly IHelperFunctions _helper;
    private readonly IAuthenticationService _service;

    public UserAuthenticationController(DataContext context,
                                        IHelperFunctions helper,
                                        IAuthenticationService service)
    {
        _context = context;
        _helper = helper;
        _service = service;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO UserRegisterRequest)
    {
        var RegisterProcess = await _service.RegisterUser(UserRegisterRequest);
        if(RegisterProcess.IsSuccess)
        {
            return Ok(new { token = RegisterProcess.Token });
        }
        else
        {
            return BadRequest(RegisterProcess.ErrorMessage);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO UserLoginRequest)
    {
        var LoginProcess = await _service.LoginUser(UserLoginRequest);
        if(LoginProcess.IsSuccess)
        {
            return Ok(new { token = LoginProcess.Token });
        }
        else
        {
            return BadRequest(LoginProcess.ErrorMessage);
        }
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