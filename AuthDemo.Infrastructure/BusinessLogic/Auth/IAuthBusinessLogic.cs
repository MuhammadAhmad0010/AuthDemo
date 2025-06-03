using AuthDemos.Core.DTOs.Auth;
using AuthDemos.Core.DTOs.Response;

namespace AuthDemo.Infrastructure.BusinessLogic.Auth
{
    public interface IAuthBusinessLogic
    {
        Task<ResponseDTO> RegisterAsync(UserRegisterDTO registerDTO);
        Task<ResponseDTO> LogInAsync(UserRegisterDTO registerDTO);
        Task<ResponseDTO> GetAccessTokenFromRefreshToken(string refreshToken);
    }
}
