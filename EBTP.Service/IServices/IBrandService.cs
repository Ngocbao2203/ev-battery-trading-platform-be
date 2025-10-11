using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IBrandService
    {
        Task<Result<IList<BrandDTO>>> GetAllAsync();
        Task<Result<BrandDTO>> GetByIdAsync(Guid id);
        Task<Result<object>> CreateAsync(CreateBrandDTO dto);
        Task<Result<object>> UpdateAsync(UpdateBrandDTO dto);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
