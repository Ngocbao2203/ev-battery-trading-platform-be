using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        IAuthRepository authRepository { get; }
        IBrandRepository brandRepository { get; }
        IPackageRepository packageRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
