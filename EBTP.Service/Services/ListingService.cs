using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Repository.Enum;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Dashboard;
using EBTP.Service.DTOs.Listing;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly IPaymentService _paymentService;
        private static string FOLDER = "listings";

        public ListingService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICloudinaryService cloudinaryService, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryService = cloudinaryService;
            _paymentService = paymentService;
        }

        public async Task<Result<List<ListingDTO>>> GetAllAsync(string? search, int pageIndex, int pageSize, decimal? from, decimal? to, ListingStatusEnum? listingStatusEnum, CategoryEnum? categoryEnum)
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

        public async Task<Result<List<ListingDTO>>> GetListingsByStatusAsync(int pageIndex, int pageSize, decimal? from, decimal? to, StatusEnum? status)
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
            var result = _mapper.Map<ListingDTO>(await _unitOfWork.listingRepository.GetListingById(id));
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

        public async Task<Result<List<ListingDTO>>> GetMyListingsAsync(int pageIndex, int pageSize)
        {
            try
            {
                // Lấy token từ header giống CreateAsync
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return new Result<List<ListingDTO>>()
                    {
                        Error = 1,
                        Message = "Token not found",
                        Data = null
                    };
                }

                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    return new Result<List<ListingDTO>>()
                    {
                        Error = 1,
                        Message = "Invalid token",
                        Data = null
                    };
                }

                var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);

                // Gọi repository với userId từ token
                var listings = await _unitOfWork.listingRepository.GetListingsByUserId(userId, pageIndex, pageSize);
                var result = _mapper.Map<List<ListingDTO>>(listings);

                return new Result<List<ListingDTO>>()
                {
                    Error = 0,
                    Message = "Lấy danh sách bài đăng của bạn thành công",
                    Count = result.Count,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ListingDTO>>()
                {
                    Error = 1,
                    Message = $"Lỗi khi lấy danh sách bài đăng: {ex.Message}",
                    Data = null
                };
            }
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
                var canUseFree = await _unitOfWork.listingRepository.CheckIsFirstListing(userId);
                var package = await _unitOfWork.packageRepository.GetByIdAsync((Guid)createListingDTO.PackageId);
                if (package.PackageType == PackageTypeEnum.Free && !canUseFree)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Chỉ được tạo một bài đăng miễn phí.",
                        Data = null
                    };
                }
                if (createListingDTO.ListingImages != null && createListingDTO.ListingImages.Count > 10)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Chỉ được tải lên tối đa 10 ảnh.",
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
                if (package.PackageType == PackageTypeEnum.Free)
                {
                    listing.PaymentStatus = PaymentStatusEnum.Success;
                }
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
        public async Task<Result<string>> CreateVnPayUrlAsync(Guid listingId, HttpContext httpContext)
        {
            var listing = await _unitOfWork.listingRepository.GetListingById(listingId);
            if (listing == null)
            {
                return new Result<string>
                {
                    Error = 1,
                    Message = "Không tìm thấy đơn hàng với ListingId tương ứng",
                    Data = null
                };
            }

            var totalAmount = listing.Package.Price;
            var transactionNo = Guid.NewGuid().ToString("N");

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                TransactionNo = transactionNo,
                ListingId = listingId,
                CreatedBy = listing.UserId,
                PaymentMethod = PaymentMethodEnum.VnPay,
                CreationDate = DateTime.UtcNow.AddHours(7),
                CreatedAt = DateTime.UtcNow.AddHours(7),
                IsDeleted = false
            };

            await _unitOfWork.paymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangeAsync();

            var url = await _paymentService.CreatePaymentUrl(transactionNo, totalAmount, httpContext);

            return new Result<string>()
            {
                Error = 0,
                Message = "Tạo URL thanh toán thành công",
                Data = url
            };
        }
        public async Task<Result<string>> RetryPayment(Guid listingId, HttpContext httpContext)
        {
            var listing = await _unitOfWork.listingRepository.GetListingById(listingId);
            if (listing == null)
            {
                return new Result<string>
                {
                    Error = 1,
                    Message = "Không tìm thấy bài đăng.",
                    Data = null
                };
            }

            if (listing.Status != StatusEnum.Expired)
            {
                return new Result<string>
                {
                    Error = 1,
                    Message = "Bài đăng chưa hết hạn, không thể thanh toán lại.",
                    Data = null
                };
            }

            var package = await _unitOfWork.packageRepository.GetByIdAsync((Guid)listing.PackageId);
            if (package == null)
            {
                return new Result<string>
                {
                    Error = 1,
                    Message = "Không tìm thấy gói đăng của bài viết.",
                    Data = null
                };
            }

            var totalAmount = package.Price;
            var transactionNo = Guid.NewGuid().ToString("N"); // ✅ mã mới hoàn toàn

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                TransactionNo = transactionNo,
                ListingId = listingId,
                CreatedBy = listing.UserId,
                PaymentMethod = PaymentMethodEnum.VnPay,
                CreationDate = DateTime.UtcNow.AddHours(7),
                CreatedAt = DateTime.UtcNow.AddHours(7),
                IsDeleted = false
            }; await _unitOfWork.paymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangeAsync();

            var url = await _paymentService.CreatePaymentUrl(transactionNo, totalAmount, httpContext);

            return new Result<string>
            {
                Error = 0,
                Message = "Tạo URL thanh toán lại thành công.",
                Data = url
            };
        }
        public async Task<Result<object>> HandleVnPayReturnAsync(IQueryCollection query)
        {
            var transactionNo = query["vnp_TxnRef"].ToString();
            if (string.IsNullOrEmpty(transactionNo))
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Mã giao dịch không hợp lệ.",
                    Data = null
                };
            }

            var payment = await _unitOfWork.paymentRepository
                .GetByTransactionNoAsync(transactionNo);

            if (payment == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy giao dịch thanh toán tương ứng.",
                    Data = null
                };
            }

            var isValid = await _paymentService.ValidateReturnData(query);
            var responseCode = query["vnp_ResponseCode"].ToString();

            payment.BankCode = query["vnp_BankCode"];
            payment.ResponseCode = responseCode;
            payment.SecureHash = query["vnp_SecureHash"];
            payment.RawData = string.Join("&", query.Select(x => $"{x.Key}={x.Value}"));
            payment.ModificationDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.paymentRepository.Update(payment);

            var listing = await _unitOfWork.listingRepository.GetListingById((Guid)payment.ListingId);

            if (listing == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy bài đăng tương ứng.",
                    Data = null
                };
            }
            var userId = listing.UserId;

            if (isValid && responseCode == "00")
            {
                listing.Status = StatusEnum.Pending;
                listing.PaymentStatus = PaymentStatusEnum.Success;
                listing.ActivatedAt = null;
                listing.ExpiredAt = null;
                listing.ModificationDate = DateTime.UtcNow.AddHours(7);
                _unitOfWork.listingRepository.Update(listing);

                var newTransaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    Amount = listing.Package.Price,
                    UserId = (Guid)userId,
                    ListingId = listing.Id,
                    PaymentId = payment.Id,
                    PackageId = (Guid)listing.PackageId,
                    Currency = "VND",
                    PaymentMethod = "Online",
                    Status = PaymentStatusEnum.Success,
                    TransactionDate = DateTime.UtcNow.AddHours(7),
                    Notes = @$"Thanh toán thành công bài đăng ""{listing.Title}""",
                    IsDeleted = false,
                    CreationDate = DateTime.UtcNow.AddHours(7)
                };
                await _unitOfWork.transactionRepository.AddAsync(newTransaction);
            }
            else
            {
                listing.PaymentStatus = PaymentStatusEnum.Failed;
                listing.Status = StatusEnum.Pending;
                listing.ModificationDate = DateTime.UtcNow.AddHours(7);
                _unitOfWork.listingRepository.Update(listing);

                var failedTransaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    Amount = listing.Package.Price,
                    UserId = (Guid)userId,
                    ListingId = listing.Id,
                    PaymentId = payment.Id,
                    PackageId = (Guid)listing.PackageId,
                    Currency = "VND",
                    PaymentMethod = "Online",
                    Status = PaymentStatusEnum.Canceled,
                    Notes = @$"Thanh toán thất bại bài đăng ""{listing.Title}""",
                    CreatedBy = userId,
                    IsDeleted = false,
                    CreationDate = DateTime.UtcNow.AddHours(7)
                };
                await _unitOfWork.transactionRepository.AddAsync(failedTransaction); await _unitOfWork.SaveChangeAsync();
                return new Result<object>
                {
                    Error = 1,
                    Message = "Thanh toán thất bại hoặc không hợp lệ.",
                    Data = null
                };
            }

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Thanh toán thành công.",
                Data = null
            };
        }
        public async Task<Result<object>> AcceptListingAsync(Guid listingId)
        {
            var getListing = await _unitOfWork.listingRepository.GetListingById(listingId);
            if (getListing == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Không tìm thấy bài đăng",
                    Data = null
                };
            }
            if (getListing.Status == StatusEnum.Active || getListing.Status == StatusEnum.Rejected)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Bài đăng đã được xử lý",
                    Data = null
                };
            }

            if (getListing.Package.PackageType != PackageTypeEnum.Free)
            {
                if (getListing.PaymentStatus != PaymentStatusEnum.Success)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Bài đăng chưa được thanh toán",
                        Data = null
                    };
                }
            }
            getListing.Status = StatusEnum.Active;
            getListing.ModificationDate = DateTime.UtcNow.AddHours(7);
            getListing.ActivatedAt = DateTime.UtcNow.AddHours(7);
            getListing.ExpiredAt = DateTime.UtcNow.AddHours(7).AddDays(getListing.Package.DurationInDays);
            _unitOfWork.listingRepository.Update(getListing);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Duyệt bài đăng thành công",
                Data = null
            };
        }

        public async Task<Result<object>> RejectListingAsync(Guid listingId, ResonRejectListingEnum reason, string descriptionReject)
        {
            var getListing = await _unitOfWork.listingRepository.GetListingById(listingId);

            if (getListing == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Không tìm thấy bài đăng",
                    Data = null
                };
            }
            if (getListing.Status == StatusEnum.Rejected || getListing.Status == StatusEnum.Active)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Bài đăng đã được xử lý",
                    Data = null
                };
            }

            if (getListing.Package.PackageType != PackageTypeEnum.Free)
            {
                if (getListing.PaymentStatus != PaymentStatusEnum.Success)
                {
                    return new Result<object>()
                    {
                        Error = 1,
                        Message = "Bài đăng chưa được thanh toán",
                        Data = null
                    };
                }
            }
            getListing.Status = StatusEnum.Rejected;
            getListing.ModificationDate = DateTime.UtcNow.AddHours(7);
            getListing.ResonReject = reason;
            getListing.DescriptionReject = descriptionReject;
            _unitOfWork.listingRepository.Update(getListing);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Từ chối bài đăng thành công",
                Data = null
            };
        }
        public async Task<Result<ListingDTO>> UpdateAsync(UpdateListingDTO updateListingDTO)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token == null)
                    return new Result<ListingDTO>() { Error = 1, Message = "Token not found", Data = null };

                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                    return new Result<ListingDTO>() { Error = 1, Message = "Invalid token", Data = null };

                var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);

                var listing = await _unitOfWork.listingRepository.GetListingById(updateListingDTO.Id);
                if (listing == null)
                {
                    return new Result<ListingDTO>
                    {
                        Error = 1,
                        Message = "Listing không tồn tại"
                    };
                }
                 
                if (listing.UserId != userId)
                {
                    return new Result<ListingDTO>
                    {
                        Error = 1,
                        Message = "Bạn không có quyền sửa bài đăng này"
                    };
                }

                listing.ListingImages ??= new List<ListingImage>();
                if (updateListingDTO.ImagesToAdd?.Any() == true)
                {
                    foreach (var image in updateListingDTO.ImagesToAdd)
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
                if (updateListingDTO.ImagesToRemove?.Any() == true)
                {
                    var imagesToMove = listing.ListingImages
                        .Where(i => updateListingDTO.ImagesToRemove.Contains(i.Id))
                        .ToList();
                    foreach (var image in imagesToMove)
                    {
                        await _cloudinaryService.DeleteImageAsync(image.ImageUrl);
                        await _unitOfWork.listingImageRepository.RemoveImage(image);
                    }
                }
                listing.Status = StatusEnum.Pending;

                _unitOfWork.listingRepository.Update(listing);
                await _unitOfWork.SaveChangeAsync();

                return new Result<ListingDTO>()
                {
                    Error = 0,
                    Message = "Cập nhật bài đăng thành công",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Result<ListingDTO>()
                {
                    Error = 1,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<Result<object>> ConfirmedSold(Guid listingId)
        {
            try
            {
                var listing = await _unitOfWork.listingRepository.GetListingById(listingId);
                if (listing == null)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Không tìm thấy bài đăng.",
                        Count = 0
                    };
                }

                if (listing.Status == StatusEnum.Sold)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Bài đăng này đã được xác nhận bán trước đó.",
                        Count = 0
                    };
                }

                listing.Status = StatusEnum.Sold;
                listing.ModificationDate = DateTime.UtcNow.AddHours(7);

                _unitOfWork.listingRepository.Update(listing);
                await _unitOfWork.SaveChangeAsync();

                return new Result<object>
                {
                    Error = 0,
                    Message = "Xác nhận bán thành công. Bài đăng đã được ẩn.",
                    Count = 1,
                    Data = new { listing.Id, listing.Title, listing.ListingStatus }
                };
            }
            catch (Exception ex)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = $"Lỗi khi xác nhận bán: {ex.Message}",
                    Count = 0
                };
            }
        }
        //Hangfire
        public async Task AutoChangeStatusWhenListingExpiredAsync()
        {
            var expiredListings = await _unitOfWork.listingRepository.CheckListingExpired();
            if (!expiredListings.Any())
                return;

            foreach (var listing in expiredListings)
            {
                listing.Status = StatusEnum.Expired;
            }

            await _unitOfWork.SaveChangeAsync();
        }
        public async Task<Result<ListingDashboard>> ListingDashboard()
        {
            var getTotalOldCars = await _unitOfWork.listingRepository.GetListingForDashBoard(CategoryEnum.ElectricCar, ListingStatusEnum.Used);
            var getTotalNewCars = await _unitOfWork.listingRepository.GetListingForDashBoard(CategoryEnum.ElectricCar, ListingStatusEnum.New);
            var getTotalNewBikes = await _unitOfWork.listingRepository.GetListingForDashBoard(CategoryEnum.ElectricMotorbike, ListingStatusEnum.New);
            var getTotalOldBikes = await _unitOfWork.listingRepository.GetListingForDashBoard(CategoryEnum.ElectricMotorbike, ListingStatusEnum.Used);
            var getTotalNewBateries = await _unitOfWork.listingRepository.GetListingForDashBoard(CategoryEnum.RemovableBattery, ListingStatusEnum.New);
            var getTotalOldBateries = await _unitOfWork.listingRepository.GetListingForDashBoard(CategoryEnum.RemovableBattery, ListingStatusEnum.Used);
            var getAllListing = await _unitOfWork.listingRepository.GetAllAsync();

            return new Result<ListingDashboard>()
            {
                Error = 0,
                Message = "Lấy dữ liệu dashboard thành công",
                Data = new ListingDashboard
                {
                    TotalListings = getAllListing.Count(),
                    TotalOldCars = getTotalOldCars.Count(),
                    TotalNewCars = getTotalNewCars.Count(),
                    TotalNewBikes = getTotalNewBikes.Count(),
                    TotalOldBikes = getTotalOldBikes.Count(),
                    TotalNewBateries = getTotalNewBateries.Count(),
                    TotalOldBateries = getTotalOldBateries.Count()
                }
            };
        }

        public async Task<Result<object>> DeleteAsync(Guid listingId)
        {
            var listing = await _unitOfWork.listingRepository.GetByIdAsync(listingId);
            if (listing == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy tin đăng",
                    Count = 0,
                    Data = null
                };
            }

            // Soft-delete; change to hard-delete if desired
            listing.IsDeleted = true;
            _unitOfWork.listingRepository.Update(listing);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Xóa tin đăng thành công",
                Count = 1,
                Data = null
            };
        }
    }
}