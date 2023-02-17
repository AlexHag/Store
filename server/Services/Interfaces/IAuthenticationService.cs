using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.Services;

public interface IAuthenticationService
{
    public Task<ControllerServiceResponse> RegisterUser(UserRegisterDTO UserRegisterRequest);
    public Task<ControllerServiceResponse> LoginUser(UserLoginDTO UserLoginRequest);
    public Task<ControllerServiceResponse> GetUserInfo(HttpContext httpContext);
}