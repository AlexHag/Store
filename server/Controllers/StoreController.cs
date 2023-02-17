using server.Services;
using server.Context;
using server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class StoreController : ControllerBase
{
    private DataContext _context;
    private readonly IConfiguration _config;
    private readonly IHelperFunctions _helper;

    public StoreController(DataContext context, IConfiguration config, IHelperFunctions helper)
    {
        _context = context;
        _config = config;
        _helper = helper;
    }

    [HttpPost]
    [Authorize]
    [Route("getstores")]
    public IActionResult GetStores([FromBody] string StoreName)
    {
        // var userId = _helper.GetRequestUserId(HttpContext);
        // var user = _context.Users.Find(userId);
        // if (user == null) return BadRequest("User not found from JWT claim. This should not be possible.");

        // if(user.Role.ToLower() != "storeowner") return BadRequest("Ownly store owners can create a new store");

        // var newStore = new Store{
        //     Id = Guid.NewGuid(),
        //     Name = StoreName,
        //     StoreOwnerId = user.Id
        // };

        // _context.Stores.Add(newStore);
        // _context.SaveChanges();
        return Ok();
    }
}