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
    }
}
