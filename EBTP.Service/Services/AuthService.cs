using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using EBTP.Repository.Repositories;
using EBTP.Service.DTOs.Auth;
using EBTP.Service.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Authenticator> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _unitOfWork.userRepository.GetUserByEmail(loginDTO.Email);

                if (user == null)
                {
                    throw new KeyNotFoundException("Email sai hoặc tài khoản không tồn tại.");
                }

                if (!user.IsVerified)
                {
                    throw new InvalidOperationException("Tài khoản chưa được kích hoạt. Vui lòng xác nhận email.");
                }
                if (user.Status.ToString() != "Active")
                {
                    throw new InvalidOperationException("Tài khoản đã bị khóa. Vui lòng liên hệ với trang web để được giải quyết.");
                }
                if (!BCrypt.Net.BCrypt.Verify(loginDTO.PasswordHash, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Mật khẩu sai.");
                }

                // Generate JWT token
                var token = await GenerateJwtToken(user);

                await _unitOfWork.SaveChangeAsync();
                return token;
            }
            catch (KeyNotFoundException ex)
            {
                // Handle cases where the user is not found
                throw new ApplicationException("Email sai hoặc tài khoản không tồn tại.", ex);
            }
            catch (InvalidOperationException ex)
            {
                // Handle cases where the account is not verified
                throw new ApplicationException("Tài khoản chưa được kích hoạt. Vui lòng xác nhận email.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle cases where the password is invalid
                throw new ApplicationException("Mật khẩu sai.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Xảy ra lỗi trong quá trình đăng nhập.", ex);
            }
        }
        private async Task<Authenticator> GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("id", user.Id.ToString()), // Ensuring UserId claim is added
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role?.RoleName),
        new Claim(ClaimTypes.Name, user.UserName)
    };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120), // Token expiration set to 120 minutes
            signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid().ToString();
            await _unitOfWork.authRepository.UpdateRefreshToken(user.Id, refreshToken);

            return new Authenticator
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }
    }
    //create
}
