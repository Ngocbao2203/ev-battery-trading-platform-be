using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Thumbnail { get; set; }
        public string Email { get; set; }
        public string? RefreshToken { get; set; }
        public StatusEnum Status { get; set; }
        public string? Otp { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime? OtpExpiryTime { get; set; }
        public string? VerificationToken { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public string? Provider { get; set; }
        public string? ProviderKey { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
