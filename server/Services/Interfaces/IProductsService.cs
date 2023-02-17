using server.Models;

namespace server.Services;

interface IProductsService
{
    public Task<bool> AddProduct(AddProductDTO AddProductRequest, HttpContext context);
}