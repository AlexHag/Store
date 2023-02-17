using server.Models;

namespace server.Services;

public interface IAuthenticationService
{
    public Task<AuthenticationServiceResponse> RegisterUser(UserRegisterDTO UserRegisterRequest);
    public Task<AuthenticationServiceResponse> LoginUser(UserLoginDTO UserLoginRequest);
}