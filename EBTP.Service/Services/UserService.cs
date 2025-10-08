using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using EBTP.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _unitOfWork.userRepository.GetUserByEmail(email);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.userRepository.UpdateAsync(user);
        }
    }
}
