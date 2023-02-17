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

    public async Task<bool> AddProduct(AddProductDTO AddProductRequest, HttpContext httpContext)
    {
        var userId = _helper.GetRequestUserId(httpContext);
        var user = _context.Users.Find(userId);
        if (user == null) return false;

        if(user.Role.ToLower() != "storeowner") return false;

        var productExists = _context.Products
            .Where(p => p.Name == AddProductRequest.Name)
            .FirstOrDefault();
        if(productExists != null) return false;

        await _context.Products.AddAsync(new Product
        {
            Id = Guid.NewGuid(),
            Name = AddProductRequest.Name,
            Description = AddProductRequest.Description,
            ImageUrl = AddProductRequest.ImageUrl,
            Price = AddProductRequest.Price,
            Quantity = AddProductRequest.Quantity,
            Category = AddProductRequest.Category,
            StoreId = user.StoreId
        });
        await _context.SaveChangesAsync();
        
        return true;
    }
}