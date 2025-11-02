using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IReportService
    {
        Task<Result<List<ReportDTO>>> GetAllReports(int pageIndex, int pageSize);
        Task<Result<ReportDTO>> GetById(Guid id);
        Task<Result<List<ReportDTO>>> GetReportsByListingId(Guid listingId, int pageIndex, int pageSize);
        Task<Result<List<ReportDTO>>> GetReportByUserId(Guid userId, int pageIndex, int pageSize);
        Task<Result<object>> SendReport(CreateReportDTO reportDTO);
        Task<Result<object>> DeleteReport(Guid id);
    }
}
