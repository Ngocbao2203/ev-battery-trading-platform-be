using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<List<Report>> GetReports(int pageIndex, int pageSize);
        Task<Report> GetReportById(Guid id);
        Task<List<Report>> GetReportsByUserId(Guid userId, int pageIndex, int pageSize);
        Task<List<Report>> GetReportsByListingId(Guid listingId, int pageIndex, int pageSize);
    }
}
