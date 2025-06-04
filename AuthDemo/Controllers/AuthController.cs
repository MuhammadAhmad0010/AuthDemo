using AuthDemo.Infrastructure.BusinessLogic.Auth;
using AuthDemos.Core.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AuthController(IAuthBusinessLogic authBusinessLogic)
        : ControllerBase
    {
        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterDTO requestDTO)
        {
            var response = await authBusinessLogic.RegisterAsync(requestDTO);
            return Ok(response);
        }

        [HttpPost]
        [ActionName("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] UserLogInDTO requestDTO)
        {
            var response = await authBusinessLogic.LogInAsync(requestDTO);
            return Ok(response);
        }

        [HttpPost]
        [ActionName("Refresh")]
        public async Task<IActionResult> GetAccessToken([FromBody]string refreshToken)
        {
            var response = await authBusinessLogic.GetAccessTokenFromRefreshToken(refreshToken);
            return Ok(response);
        }

    }
}
