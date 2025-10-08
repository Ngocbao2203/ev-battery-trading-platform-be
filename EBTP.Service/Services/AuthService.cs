using EBTP.Repository.Entities;
using EBTP.Repository.Enum;
using EBTP.Repository.IRepositories;
using EBTP.Repository.Repositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Auth;
using EBTP.Service.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _emailService = emailService;
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
        public async Task<Result<object>> RegisterUserAsync(UserRegistrationDTO userRegistrationDto)
        {
            try
            {
                if (await _unitOfWork.userRepository.ExistsAsync(u => u.Email == userRegistrationDto.Email))
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Email đã tồn tại.",
                        Data = null
                    };
                }
                var otp = GenerateOtp();
                //var uploadResult = await _cloudinaryService.UploadProductImage(userRegistrationDto.Thumbnail, FOLDER);
                var user = new User
                {
                    UserName = userRegistrationDto.UserName,
                    Email = userRegistrationDto.Email,
                    PasswordHash = HashPassword(userRegistrationDto.PasswordHash),
                    PhoneNumber = userRegistrationDto.PhoneNo,
                    Status = StatusEnum.Pending,
                    Otp = otp,
                    RoleId = 2,
                    CreationDate = DateTime.Now.AddHours(7),
                    OtpExpiryTime = DateTime.UtcNow.AddHours(7).AddMinutes(10)
                };

                await _unitOfWork.userRepository.AddAsync(user);
                await _emailService.SendOtpEmailAsync(user.Email, otp);
                return new Result<object>
                {
                    Error = 0,
                    Message = "Đăng ký tài khoản thành công. Vui lòng kiểm tra email để lấy mã OPT. ",
                    Data = null
                };
            }
            catch (ArgumentNullException ex)
            {
                // Handle cases where required information is missing
                throw new ApplicationException("Vui lòng điền các thông tin cần thiết.", ex);
            }
            catch (InvalidOperationException ex)
            {
                // Handle cases where an operation is invalid, such as duplicate user registration
                throw new ApplicationException("Thao tác không hợp lệ trong quá trình đăng ký người dùng.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Xảy ra lỗi trong quá trình đăng ký.", ex);
            }
        }
        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            try
            {
                var user = await _unitOfWork.userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy người dùng.");
                }

                if (user.Otp != otp || user.OtpExpiryTime < DateTime.UtcNow.AddHours(7))
                {
                    return false;
                }

                user.IsVerified = true;
                user.Otp = null;
                user.OtpExpiryTime = null;
                user.Status = StatusEnum.Active; // Update status to Active

                await _unitOfWork.userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (KeyNotFoundException ex)
            {
                // Handle cases where the user is not found
                throw new ApplicationException("Không tìm thấy người dùng để xác minh OTP.", ex);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new ApplicationException("Đã xảy ra lỗi khi xác minh OTP.", ex);
            }
        }
        public async Task<Result<object>> ResendOtpAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.userRepository
                    .FindByEmail(email);

                if (user == null)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Email không tồn tại.",
                        Data = null
                    };
                }

                if (user.IsVerified)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Tài khoản đã được xác minh.",
                        Data = null
                    };
                }

                string otp;
                if (user.Otp != null && user.OtpExpiryTime.HasValue && user.OtpExpiryTime > DateTime.UtcNow.AddHours(7))
                {
                    otp = user.Otp;
                }
                else
                {
                    otp = GenerateOtp();
                    user.Otp = otp;
                    user.OtpExpiryTime = DateTime.UtcNow.AddHours(7).AddMinutes(10);

                    await _unitOfWork.userRepository.UpdateAsync(user);
                    await _unitOfWork.SaveChangeAsync();
                }
                await _emailService.SendOtpEmailAsync(email, otp);
                return new Result<object>()
                {
                    Error = 0,
                    Message = "Gửi lại mã xác minh tới email thành công.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Đã xảy ra lỗi khi gửi lại mã xác minh.",
                    Data = null
                };
            }
        }
        public async Task<bool> VerifyOtpAndCompleteRegistrationAsync(string email, string otp)
        {
            var user = await _unitOfWork.userRepository.GetUserByEmail(email);
            if (user == null || user.Otp != otp || user.OtpExpiryTime < DateTime.UtcNow.AddHours(7))
    {
                return false;
            }

            user.IsVerified = true;
            user.Status = user.RoleId == 2 ? StatusEnum.Pending : StatusEnum.Active;
            user.Otp = "";
            user.OtpExpiryTime = null;

            await _unitOfWork.userRepository.UpdateAsync(user);
            return true;
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
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private string GenerateOtp()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[4];
                rng.GetBytes(byteArray);
                var otp = BitConverter.ToUInt32(byteArray, 0) % 1000000;
                return otp.ToString("D6");
            }
        }
    }
}
