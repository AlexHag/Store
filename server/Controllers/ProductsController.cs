using server.Services;
using server.Context;
using server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class ProductsController : ControllerBase
{
    private DataContext _context;
    private readonly IConfiguration _config;
    private readonly IHelperFunctions _helper;

    public ProductsController(DataContext context, IConfiguration config, IHelperFunctions helper)
    {
        _context = context;
        _config = config;
        _helper = helper;
    }

    [HttpPost]
    [Authorize]
    [Route("addproduct")]
    public IActionResult AddProduct([FromBody] AddProductDTO AddProductRequest)
    {
        
        return Ok();
    }
}