using EBTP.Service.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailDTO request);
        Task SendOtpEmailAsync(string email, string otp);
        Task SendPendingEmailAsync(string email);
        Task SendApprovalEmailAsync(string email);
    }
}
