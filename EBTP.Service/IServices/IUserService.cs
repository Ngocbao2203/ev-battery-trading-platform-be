using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);
        Task UpdateUserAsync(User user);
    }
}
