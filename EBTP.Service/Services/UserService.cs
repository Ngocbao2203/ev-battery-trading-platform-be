using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.User;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private static string FOLDER = "thumbnail";

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _unitOfWork.userRepository.GetUserByEmail(email);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.userRepository.UpdateAsync(user);
        }
        public async Task<Result<UserDTO>> GetCurrentUserById()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
                return new Result<UserDTO>() { Error = 1, Message = "Token not found", Data = null };

            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return new Result<UserDTO>() { Error = 1, Message = "Invalid token", Data = null };
            var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            var result = _mapper.Map<UserDTO>(await _unitOfWork.userRepository.GetAllUserById(userId));
            return new Result<UserDTO>() { Error = 0, Message = "Lấy thông tin người dùng thành công", Data = result };
        }
        public async Task<Result<object>> UpdateUserInfoAsync(UpdateInfoUserDTO updateDto)
        {
            try
            {
                var user = await _unitOfWork.userRepository.GetUserById(updateDto.Id);

                if (user == null)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Không tìm thấy người dùng.",
                        Data = null
                    };
                }

                user.UserName = updateDto.UserName ?? user.UserName;
                user.Email = updateDto.Email ?? user.Email;
                user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
                user.DateOfBirth = updateDto.DateOfBirth ?? user.DateOfBirth;

                // Upload ảnh mới nếu có
                if (updateDto.Thumbnail != null && updateDto.Thumbnail.Length > 0)
                {
                    var uploadResult = await _cloudinaryService.UploadProductImage(updateDto.Thumbnail, FOLDER);
                    user.Thumbnail = uploadResult.SecureUrl.ToString();
                }

                await _unitOfWork.userRepository.UpdateAsync(user);

                return new Result<object>
                {
                    Error = 0,
                    Message = "Cập nhật thông tin người dùng thành công.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = $"Có lỗi xảy ra: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
