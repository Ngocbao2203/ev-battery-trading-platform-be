using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetAllUserById(Guid id);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
        Task<User?> FindByEmail(string email);
        Task<User> GetUserByResetToken(string resetToken);
        Task UpdateAsync(User user);
        Task AddAsync(User user);
    }
}
