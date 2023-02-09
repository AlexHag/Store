
namespace server.Models;

public class Store
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid StoreOwnerId { get; set; }
}