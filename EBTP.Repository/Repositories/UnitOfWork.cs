using EBTP.Repository.Data;
using EBTP.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IBrandRepository _brandRepository;

        public UnitOfWork(AppDbContext context, IUserRepository userRepository, IAuthRepository authRepository, IBrandRepository brandRepository, IPackageRepository packageRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _authRepository = authRepository;
            _brandRepository = brandRepository;
            _packageRepository = packageRepository;
        }

        public IUserRepository userRepository => _userRepository;
        public IAuthRepository authRepository => _authRepository;
        public IBrandRepository brandRepository => _brandRepository;
        public IPackageRepository packageRepository => _packageRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
