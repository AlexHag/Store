using server.Models;
using server.Services;
using server.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    private DataContext _context;
    private readonly IConfiguration _config;
    private readonly IHelperFunctions _helper;

    public SecureController(DataContext context, IConfiguration config, IHelperFunctions helper)
    {
        _context = context;
        _config = config;
        _helper = helper;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        var userId = _helper.GetRequestUserId(HttpContext);
        var user = _context.Users.Find(userId);
        if (user == null) return BadRequest("User not found from JWT claim. This should not be possible.");
        
        return Ok($"Hello {user.Email}. Your role is: {user.Role}");
    }
}