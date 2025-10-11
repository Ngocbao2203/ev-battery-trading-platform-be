using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Package;
using EBTP.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class PakageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PakageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<PackageDTO>>> GetAllAsync()
        {
            var result = _mapper.Map<List<PackageDTO>>(await _unitOfWork.packageRepository.GetAllPackageAsync());
            return new Result<List<PackageDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sách gói thành công",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<Result<PackageDTO>> GetByIdAsync(Guid id)
        {
            var result = _mapper.Map<PackageDTO>(await _unitOfWork.packageRepository.GetPackageByIdAsync(id));
            return new Result<PackageDTO>()
            {
                Error = 0,
                Message = "Lấy danh sách gói thành công",
                Count = 1,
                Data = result
            };
        }
        public async Task<Result<object>> CreateAsync(CreatePakageDTO dto)
        {
            var checkName = await _unitOfWork.packageRepository.GetByNameAsync(dto.Name);
            if (checkName != null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Tên gối đã tồn tại",
                    Count = 0,
                    Data = null
                };
            }

            var package = _mapper.Map<Package>(dto);
            await _unitOfWork.packageRepository.AddAsync(package);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Thêm gói đăng tin thành công",
                Count = 1,
                Data = null
            };
        }
        public async Task<Result<object>> UpdateAsync(UpdatePackageDTO dto)
        {
            var package = await _unitOfWork.packageRepository.GetByIdAsync(dto.Id);
            if (package == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Không tìm thấy gói đăng tin",
                    Count = 0,
                    Data = null
                };
            }

            var checkName = await _unitOfWork.packageRepository.GetByNameAsync(dto.Name);
            if (checkName != null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Tên gói đã tồn tại",
                    Count = 0,
                    Data = null
                };
            }
            var result = _mapper.Map(dto, package);
            _unitOfWork.packageRepository.Update(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật gói thành công",
                Count = 1,
                Data = null
            };
        }

        public async Task<Result<object>> DeleteAsync(Guid id)
        {
            var package = await _unitOfWork.packageRepository.GetByIdAsync(id);
            if (package == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Không tìm thấy gói",
                    Count = 0,
                    Data = null
                };
            }
            package.IsDeleted = true;
            _unitOfWork.packageRepository.Update(package);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Xóa gói thành công",
                Count = 1,
                Data = null
            };
        }
    }
}
