using System.ComponentModel.DataAnnotations;

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

public class AddProductDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]

    public int Quantity { get; set; }
    [Required]

    public string Category { get; set; }
}