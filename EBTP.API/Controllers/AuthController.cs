using EBTP.Repository.Enum;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Auth;
using EBTP.Service.IServices;
using EBTP.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IUserService userService, IEmailService emailService)
        {
            _authService = authService;
            _userService = userService;
            _emailService = emailService;
        }
        [HttpPost("user/login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDTO);
                Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/",
                    SameSite = SameSiteMode.Strict,
                });
                return Ok(new Result<object>
                {
                    Error = 0,
                    Message = "Success",
                    Data = new
                    {
                        TokenType = "Bearer",
                        token.AccessToken,
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        [HttpPost("user/register/user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userRegistrationDto)
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest(new { message = "Bạn đã đăng nhập và không thể đăng ký lại." });
            }

            try
            {
                var result = await _authService.RegisterUserAsync(userRegistrationDto);
                if (result.Error == 1)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        [HttpPost("user/otp/resend")]
        public async Task<IActionResult> ResendOtp([FromQuery] string email)
        {
            try
            {
                var result = await _authService.ResendOtpAsync(email);
                if (result.Error == 1)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        [HttpPost("user/otp/verify")]
        public async Task<IActionResult> VerifyOtp(OtpVerificationDTO otpVerificationDto)
        {
            try
            {
                var isValid = await _authService.VerifyOtpAndCompleteRegistrationAsync(otpVerificationDto.Email, otpVerificationDto.Otp);
                if (!isValid)
                {
                    return BadRequest(new { Message = "OTP không hợp lệ hoặc OTP đã hết hạn." });
                }

                var user = await _userService.GetByEmail(otpVerificationDto.Email);
                if (user != null)
                {
                    user.IsVerified = true;

                    if (user.RoleId == 3) // Role 3 is for shop
                    {
                        user.Status = StatusEnum.Pending;
                        await _emailService.SendPendingEmailAsync(user.Email);
                    }
                    else
                    {
                        user.Status = StatusEnum.Active;
                        await _emailService.SendApprovalEmailAsync(user.Email);
                    }

                    await _userService.UpdateUserAsync(user);
                }

                return Ok(new { Message = "Xác thực Email thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        [HttpPost("user/password/change")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDTO changePasswordDto)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (email == null)
                {
                    return Unauthorized(new { Message = "Token không hợp lệ." });
                }

                await _authService.ChangePasswordAsync(email, changePasswordDto);
                return Ok(new { Message = "Mật khẩu đã được thay đổi thành công, vui lòng đăng nhập lại." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("user/password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordRequestDTO forgotPasswordRequestDto)
        {
            try
            {
                await _authService.RequestPasswordResetAsync(forgotPasswordRequestDto);
                return Ok(new { Message = "Mã thông báo đặt lại mật khẩu đã được gửi thành công đến email của bạn." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("user/password/reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                await _authService.ResetPasswordAsync(resetPasswordDto);
                return Ok(new { Message = "Đã đặt lại mật khẩu thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
