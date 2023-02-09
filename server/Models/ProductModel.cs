
namespace server.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }
    public Guid StoreId { get; set; }

}