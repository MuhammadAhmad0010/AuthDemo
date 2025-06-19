using AuthDemo.Infrastructure.Dapper;
using AuthDemos.Core.DTOs.Auth;
using AuthDemos.Core.DTOs.Response;
using AuthDemos.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthDemo.Infrastructure.BusinessLogic.Auth
{
    public class AuthBusinessLogic : IAuthBusinessLogic
    {
        #region fields
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IDapperRepository<UserRefreshTokens> _userRefreshTokenRepository;
        #endregion

        #region ctor
        public AuthBusinessLogic(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            IDapperRepository<UserRefreshTokens> userRefreshTokenRepository)
        {
            _userManager = userManager;
            _config = configuration;
            _userRefreshTokenRepository = userRefreshTokenRepository;
        }

        #endregion

        #region Utilities

        private string HashToken(string token)
        {
            using (var sha256 = SHA256.Create())
            {
                var tokenBytes = Encoding.UTF8.GetBytes(token);
                var hashBytes = sha256.ComputeHash(tokenBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }
        private string GenerateAccessToken(IdentityUser user)
        {
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Name, user.UserName!)
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<string> GenerateRefreshToken(IdentityUser user)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(randomBytes) + Guid.NewGuid();

            var hashToken = HashToken(token);

            var tokenExist = (await _userRefreshTokenRepository.GetByColumnAsync(nameof(UserRefreshTokens.UserId), user.Id))
                                     .FirstOrDefault();
            if (tokenExist != null)
            {
                await _userRefreshTokenRepository.DeleteAsync(tokenExist.Id);
            }

            var entity = new UserRefreshTokens()
            {
                Token = hashToken,
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                LastIssuedDate = DateTime.UtcNow,
                UserId = user.Id
            };

            await _userRefreshTokenRepository.InsertAsync(entity);

            return token;
        }


        #endregion

        #region Methods
        public async Task<ResponseDTO> RegisterAsync(UserRegisterDTO registerDTO)
        {
            ResponseDTO response = new();
            try
            {
                var user = new IdentityUser()
                {
                    Email = registerDTO.Email,
                    UserName = registerDTO.Email
                };

                var status = await _userManager.CreateAsync(user, registerDTO.Password);

                if (!status.Succeeded)
                {
                    response.Status = false;
                    response.Message = string.Join(",", status.Errors.Select(e => e.Description));
                }
                else
                {
                    response.Message = "User registered successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseDTO> LogInAsync(UserRegisterDTO registerDTO)
        {
            ResponseDTO response = new();
            try
            {
                var userExist = await _userManager.FindByEmailAsync(registerDTO.Email);

                if (userExist != null && await _userManager.CheckPasswordAsync(userExist, registerDTO.Password))
                {
                    LogInSuccessResponseDTO logInResponse = new();
                    logInResponse.Email = registerDTO.Email;
                    logInResponse.RefreshToken = await GenerateRefreshToken(userExist);
                    logInResponse.AccessToken = GenerateAccessToken(userExist);

                    response.Data = logInResponse;

                }
                else
                {
                    response.Status = false;
                    response.Message = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseDTO> GetAccessTokenFromRefreshToken(string refreshToken)
        {
            ResponseDTO response = new();
            try
            {
                var hashedRefreshToken = HashToken(refreshToken);
                var isValidRefreshToken = (await _userRefreshTokenRepository.GetByColumnAsync(nameof(UserRefreshTokens.Token), hashedRefreshToken)).FirstOrDefault();

                if (isValidRefreshToken != null &&
                    isValidRefreshToken.ExpiresOn.Date > DateTime.UtcNow.Date)
                {
                    var user = await _userManager.FindByIdAsync(isValidRefreshToken.UserId);
                    var accessToken = GenerateAccessToken(user!);

                    response.Data = accessToken;
                }
                else
                {
                    response.Status = false;
                    response.Message = "Refresh token is not valid.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return response;
        }

        #endregion
    }
}
