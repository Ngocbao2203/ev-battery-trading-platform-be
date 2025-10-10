using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IAuthService
    {
        Task<Authenticator> LoginAsync(LoginDTO loginDTO);
        Task<Result<object>> RegisterUserAsync(UserRegistrationDTO userRegistrationDto);
        Task<bool> VerifyOtpAsync(string email, string otp);
        Task<Result<object>> ResendOtpAsync(string email);
        Task<bool> VerifyOtpAndCompleteRegistrationAsync(string email, string otp);
        Task ChangePasswordAsync(string email, ChangePasswordDTO changePasswordDto);
        //FORGOT PASSWORD
        Task RequestPasswordResetAsync(ForgotPasswordRequestDTO forgotPasswordRequestDto);
        Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);
    }
}
