using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Repository.Enum;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Listing;
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
    public class ListingService : IListingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryService _cloudinaryService;
        private static string FOLDER = "listings";

        public ListingService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<List<ListingDTO>>> GetAllAsync(string? search, int pageIndex, int pageSize, decimal from, decimal to, ListingStatusEnum? listingStatusEnum, CategoryEnum? categoryEnum)
        {
            var result = _mapper.Map<List<ListingDTO>>(await _unitOfWork.listingRepository.GetAllListings(search, pageIndex, pageSize, from, to, listingStatusEnum, categoryEnum));
            return new Result<List<ListingDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách bài đăng thành công",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<Result<List<ListingDTO>>> GetListingsByStatusAsync(int pageIndex, int pageSize, decimal from, decimal to, StatusEnum? status)
        {
            var result = _mapper.Map<List<ListingDTO>>(await _unitOfWork.listingRepository.GetListingsByStatus(pageIndex, pageSize, from, to, status));
            return new Result<List<ListingDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách bài đăng thành công",
                Count = result.Count,
                Data = result
            };
        }
        public async Task<Result<ListingDTO>> GetByIdAsync(Guid id)
        {
            var result = _mapper.Map<ListingDTO>(await _unitOfWork.listingRepository.GetByIdAsync(id));
            if (result == null)
            {
                return new Result<ListingDTO>()
                {
                    Error = 1,
                    Message = "Không tìm thấy bài đăng",
                    Data = null
                };
            }
            return new Result<ListingDTO>()
            {
                Error = 0,
                Message = "Lấy danh sách bài đăng thành công",
                Data = result
            };
        }
        public async Task<Result<object>> CreateAsync(CreateListingDTO createListingDTO)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                    return new Result<object>() { Error = 1, Message = "Token not found", Data = null };

                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return new Result<object>() { Error = 1, Message = "Invalid token", Data = null };
                var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
                var listing = _mapper.Map<Listing>(createListingDTO);

                if (createListingDTO.ListingImages != null && createListingDTO.ListingImages.Count > 5)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Chỉ được tải lên tối đa 5 ảnh.",
                        Data = null
                    };
                }

                if (createListingDTO.ListingImages != null && createListingDTO.ListingImages.Any())
                {
                    foreach (var image in createListingDTO.ListingImages)
                    {
                        var uploadResult = await _cloudinaryService.UploadProductImage(image, FOLDER);

                        if (uploadResult != null)
                        {
                            listing.ListingImages.Add(new ListingImage
                            {
                                ImageUrl = uploadResult.SecureUrl.ToString()
                            });
                        }
                    }
                }
                listing.Status = StatusEnum.Pending;
                listing.UserId = userId;
                listing.CreationDate = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.listingRepository.AddAsync(listing);
                await _unitOfWork.SaveChangeAsync();
                return new Result<object>()
                {
                    Error = 0,
                    Message = "Tạo bài đăng thành công",
                    Data = listing.Id
                };
            }
            catch (InvalidOperationException ex)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = ex.Message
                };
            }
        }
    }
}
