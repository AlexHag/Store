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
    private readonly IAuthenticationService _service;

    public UserAuthenticationController(IAuthenticationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO UserRegisterRequest)
    {
        var RegisterProcess = await _service.RegisterUser(UserRegisterRequest);
        if(RegisterProcess.IsSuccess)
        {
            return Ok(new { token = RegisterProcess.Value });
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
            return Ok(new { token = LoginProcess.Value });
        }
        else
        {
            return BadRequest(LoginProcess.ErrorMessage);
        }
    }

    [Authorize]
    [HttpGet]
    [Route("userinfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        var GetUserInfoProcess = await _service.GetUserInfo(HttpContext);
        if(GetUserInfoProcess.IsSuccess)
        {
            return Ok(GetUserInfoProcess.Value);
        }
        else
        {
            return BadRequest(GetUserInfoProcess.ErrorMessage);
        }
    }
}