using server.Models;

namespace server.Services;

public interface IHelperFunctions
{
    public string RandomString(int length);
    public string HashString(string input);
    public Guid GetRequestUserId(HttpContext context);
    public string CreateJWT(Guid userId);
    public User CreateNewUserObject(UserRegisterDTO UserRegisterRequest);
}