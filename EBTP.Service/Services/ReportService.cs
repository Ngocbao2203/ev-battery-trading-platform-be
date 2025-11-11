using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Report;
using EBTP.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWord;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private static string FOLDER = "reports";

        public ReportService(IUnitOfWork unitOfWord, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWord = unitOfWord;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<List<ReportDTO>>> GetAllReports(int pageIndex, int pageSize)
        {
            var reports = await _unitOfWord.reportRepository.GetReports(pageIndex, pageSize);

            var data = _mapper.Map<List<ReportDTO>>(reports);
            return new Result<List<ReportDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách báo cáo thành công.",
                Count = data.Count,
                Data = data
            };
        }

        public async Task<Result<ReportDTO>> GetById(Guid id)
        {
            var report = await _unitOfWord.reportRepository.GetReportById(id);
            var data = _mapper.Map<ReportDTO>(report);
            return new Result<ReportDTO>
            {
                Error = 0,
                Message = "Lấy báo cáo thành công.",
                Count = 1,
                Data = data
            };
        }

        public async Task<Result<List<ReportDTO>>> GetReportByUserId(Guid userId, int pageIndex, int pageSize)
        {
            var reports = await _unitOfWord.reportRepository.GetReportsByUserId(userId, pageIndex, pageSize);
            var data = _mapper.Map<List<ReportDTO>>(reports);
            return new Result<List<ReportDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách báo cáo theo người dùng thành công.",
                Count = data.Count,
                Data = data
            };
        }

        public async Task<Result<List<ReportDTO>>> GetReportsByListingId(Guid listingId, int pageIndex, int pageSize)
        {
            var reports = await _unitOfWord.reportRepository.GetReportsByListingId(listingId, pageIndex, pageSize);
            var data = _mapper.Map<List<ReportDTO>>(reports);
            return new Result<List<ReportDTO>>
            {
                Error = 0,
                Message = "Lấy danh sách báo cáo theo bài đăng thành công.",
                Count = data.Count,
                Data = data
            };
        }
        public async Task<Result<object>> SendReport(CreateReportDTO reportDTO)
        {
            try
            {
                var existingReports = await _unitOfWord.reportRepository
                    .GetReportsByUserId(reportDTO.UserId, 1, int.MaxValue);

                if (existingReports.Any(r => r.ListingId == reportDTO.ListingId))
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Bạn đã gửi báo cáo cho bài đăng này trước đó.",
                        Count = 0
                    };
                }

                var report = _mapper.Map<Report>(reportDTO);
                report.Id = Guid.NewGuid();
                report.ReportImages = new List<ReportImage>();
                if (reportDTO.ReportImages != null && reportDTO.ReportImages.Count > 10)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "Chỉ được tải lên tối đa 10 ảnh.",
                        Data = null
                    };
                }

                if (reportDTO.ReportImages != null && reportDTO.ReportImages.Any())
                {
                    foreach (var image in reportDTO.ReportImages)
                    {
                        var uploadResult = await _cloudinaryService.UploadProductImage(image, FOLDER);

                        if (uploadResult != null)
                        {
                            report.ReportImages.Add(new ReportImage
                            {
                                ImageUrl = uploadResult.SecureUrl.ToString()
                            });
                        }
                    }
                }
                report.CreationDate = DateTime.UtcNow.AddHours(7);

                await _unitOfWord.reportRepository.AddAsync(report);
                await _unitOfWord.SaveChangeAsync();

                return new Result<object>
                {
                    Error = 0,
                    Message = "Gửi báo cáo thành công.",
                    Count = 1
                };
            }
            catch (Exception ex)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = $"Lỗi khi gửi báo cáo: {ex.Message}",
                    Count = 0
                };
            }
        }

        public async Task<Result<object>> DeleteReport(Guid id)
        {
            var report = await _unitOfWord.reportRepository.GetReportById(id);

            if (report == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy báo cáo để xóa.",
                    Count = 0
                };
            }

            report.IsDeleted = true;
            await _unitOfWord.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Xóa báo cáo thành công.",
                Count = 1
            };
        }
    }
}
