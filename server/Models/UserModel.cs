namespace server.Models;

public class User
{
    public Guid Id  { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string Role { get; set; }
    public Guid StoreId { get; set; }
}

public class UserLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserRegisterDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}