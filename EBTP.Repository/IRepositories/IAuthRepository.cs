using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IAuthRepository : IGenericRepository<User>
    {
        Task<bool> UpdateRefreshToken(Guid userId, string refreshToken);

        Task<User> GetRefreshToken(string refreshToken);
        Task<bool> DeleteRefreshToken(Guid userId);
    }
}
