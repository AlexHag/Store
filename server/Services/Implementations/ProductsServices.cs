using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Models;

namespace server.Services;

public class ProductsService : IProductsService
{
    private DataContext _context;
    private readonly IHelperFunctions _helper;
    public ProductsService(DataContext context, IHelperFunctions helper)
    {
        _helper = helper;
        _context = context;
    }

    public async Task<IActionResult> AddProduct(AddProductDTO AddProductRequest, HttpContext httpContext)
    {
        var userId = _helper.GetRequestUserId(httpContext);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return new BadRequestObjectResult("User is null eventhough jwt token is valid. This should not happen.");

        if(user.Role.ToLower() != "storeowner") return new BadRequestObjectResult("Only store owners can add products.");
        

        var productExists = _context.Products
            .Where(p => p.Name == AddProductRequest.Name)
            .FirstOrDefault();
        if(productExists != null) return new BadRequestObjectResult($"Product with name {AddProductRequest.Name} already exist.");

        var ProductToAdd = new Product
        {
            Id = Guid.NewGuid(),
            Name = AddProductRequest.Name,
            Description = AddProductRequest.Description,
            ImageUrl = AddProductRequest.ImageUrl,
            Price = AddProductRequest.Price,
            Quantity = AddProductRequest.Quantity,
            Category = AddProductRequest.Category,
            StoreId = user.StoreId
        };

        await _context.Products.AddAsync(ProductToAdd);
        await _context.SaveChangesAsync();

        return new CreatedAtActionResult("GetProduct","Products", new { ProductId = ProductToAdd.Id }, ProductToAdd);
    }

    public async Task<IActionResult> GetProduct(Guid ProductId)
    {
        var product = await _context.Products.FindAsync(ProductId);
        if(product == null) return new NotFoundObjectResult(null);
        return new OkObjectResult(product);
    }

    public async Task<ControllerServiceResponse> GetProductFromStore(String StoreName)
    {
        var store = _context.Stores.Where(p => p.Name == StoreName).FirstOrDefault();
        if(store == null) return new ControllerServiceResponse
        {
            IsSuccess = false,
            ErrorMessage = "Store does not exist."
        };
        var products = await _context.Products.Where(p => p.StoreId == store.Id).ToListAsync();
        return new ControllerServiceResponse
        {
            IsSuccess = true,
            Value = products
        };
    }

    public Task<ControllerServiceResponse> DeleteProduct(Guid ProductId, HttpContext context)
    {
        throw new NotImplementedException();
    }
}