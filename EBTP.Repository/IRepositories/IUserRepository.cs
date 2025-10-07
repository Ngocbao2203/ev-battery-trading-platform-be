using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}
