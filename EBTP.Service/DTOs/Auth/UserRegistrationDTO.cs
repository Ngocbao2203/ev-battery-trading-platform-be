using EBTP.Service.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Auth
{
    public class UserRegistrationDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        //public IFormFile Thumbnail { get; set; }
        public string PhoneNo { get; set; }

        [Required]
        [PasswordValidation]
        public string PasswordHash { get; set; }
    }
}
