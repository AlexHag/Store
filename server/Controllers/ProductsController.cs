using server.Services;
using server.Context;
using server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _service;

    public ProductsController(IProductsService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize]
    [Route("Products/Add")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductDTO AddProductRequest)
    {
        return await _service.AddProduct(AddProductRequest, HttpContext);
    }

    [HttpGet("Product/{ProductId}")]
    public async Task<IActionResult> GetProduct(Guid ProductId)
    {
        return await _service.GetProduct(ProductId);
    }

    [HttpGet("Store/{StoreName}")]
    public async Task<IActionResult> GetProductsFromStore(string StoreName)
    {
        var GetProductsFromStoreProcess = await _service.GetProductFromStore(StoreName);
        if(GetProductsFromStoreProcess.IsSuccess)
        {
            return Ok(GetProductsFromStoreProcess.Value);
        }
        else
        {
            return NotFound(GetProductsFromStoreProcess.ErrorMessage);
        }
    }
}