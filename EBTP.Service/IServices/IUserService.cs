using EBTP.Repository.Entities;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.User;
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
        Task<Result<UserDTO>> GetCurrentUserById();
        Task<Result<object>> UpdateUserInfoAsync(UpdateInfoUserDTO updateDto);
        Task<Result<List<UserDTO>>> GetAllUsers(int pageIndex, int pageSize);
        Task<Result<UserDTO>> GetUserByUserId(Guid userId);
        Task<Result<object>> BanUser(Guid userId, string descriptionBan);
        Task<Result<object>> UnBanUser(Guid userId);
    }
}
