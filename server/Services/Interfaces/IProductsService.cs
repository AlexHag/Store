using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Services;

public interface IProductsService
{
    // Create
    public Task<IActionResult> AddProduct(AddProductDTO AddProductRequest, HttpContext context);
    
    // Read
    public Task<IActionResult> GetProduct(Guid ProductId);
    public Task<ControllerServiceResponse> GetProductFromStore(string StoreName);

    // Update [HttpPut]
    // public Task<ControllerServiceResponse> UpdateProduct(UpdateProductDTO ProductToUpdate, HttpContext context);

    //Delete
    public Task<ControllerServiceResponse> DeleteProduct(Guid ProductId, HttpContext context);
}