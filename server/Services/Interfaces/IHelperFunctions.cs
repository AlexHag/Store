using server.Models;

namespace server.Services;

public interface IHelperFunctions
{
    public Guid GetRequestUserId(HttpContext context);
}