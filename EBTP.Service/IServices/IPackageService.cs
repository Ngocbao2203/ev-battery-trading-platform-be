using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IPackageService
    {
        Task<Result<List<PackageDTO>>> GetAllAsync();
        Task<Result<PackageDTO>> GetByIdAsync(Guid id);
        Task<Result<object>> CreateAsync(CreatePakageDTO dto);
        Task<Result<object>> UpdateAsync(UpdatePackageDTO dto);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
