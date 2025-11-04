using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Brand;
using EBTP.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IList<BrandDTO>>> GetAllAsync()
        {
            var result = _mapper.Map<IList<BrandDTO>>(await _unitOfWork.brandRepository.GetAllAsync());
            return new Result<IList<BrandDTO>>()
            {
                Error = 0,
                Message = "Lấy danh sach thương hiệu thành công",
                Count = result.Count,
                Data = result
            };
        }

        public async Task<Result<BrandDTO>> GetByIdAsync(Guid id)
        {
            var result = _mapper.Map<BrandDTO>(await _unitOfWork.brandRepository.GetByIdAsync(id));
            return new Result<BrandDTO>()
            {
                Error = 0,
                Message = "Lấy danh sach thương hiệu thành công",
                Count = 1,
                Data = result
            };
        }
        public async Task<Result<object>> CreateAsync(CreateBrandDTO dto)
        {
            var existedBrand = await _unitOfWork.brandRepository.GetByNameAndTypeAsync(dto.Name, dto.Type);
            if (existedBrand != null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Thương hiệu với tên và loại này đã tồn tại",
                    Count = 0,
                    Data = null
                };
            }

            var brand = _mapper.Map<Brand>(dto);
            await _unitOfWork.brandRepository.AddAsync(brand);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Thêm thương hiệu thành công",
                Count = 1,
                Data = null
            };
        }
        public async Task<Result<object>> UpdateAsync(UpdateBrandDTO dto)
        {
            var brand = await _unitOfWork.brandRepository.GetByIdAsync(dto.Id);
            if (brand == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Không tìm thấy thương hiệu",
                    Count = 0,
                    Data = null
                };
            }

            var checkName = await _unitOfWork.brandRepository.GetByNameAsync(dto.Name);
            if (checkName != null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Tên thương hiệu đã tồn tại",
                    Count = 0,
                    Data = null
                };
            }
            var result = _mapper.Map(dto, brand);
            _unitOfWork.brandRepository.Update(result);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Cập nhật thương hiệu thành công",
                Count = 1,
                Data = null
            };
        }
        public async Task<Result<object>> DeleteAsync(Guid id)
        {
            var brand = await _unitOfWork.brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return new Result<object>()
                {
                    Error = 1,
                    Message = "Không tìm thấy thương hiệu",
                    Count = 0,
                    Data = null
                };
            }
            _unitOfWork.brandRepository.Remove(brand);
            await _unitOfWork.SaveChangeAsync();
            return new Result<object>()
            {
                Error = 0,
                Message = "Xóa thương hiệu thành công",
                Count = 1,
                Data = null
            };
        }
    }
}
