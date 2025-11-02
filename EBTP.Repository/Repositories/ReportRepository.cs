using EBTP.Repository.Data;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        private readonly AppDbContext _context;
        public ReportRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Report> GetReportById(Guid id)
        {
            return await _context.Report
                .Include(r => r.User)
                .Include(r => r.Listing)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Report>> GetReports(int pageIndex, int pageSize)
        {
            return await _context.Report
                .Include(r => r.User)
                .Include(r => r.Listing)
                .OrderByDescending(r => r.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Report>> GetReportsByListingId(Guid listingId, int pageIndex, int pageSize)
        {
            return await _context.Report
                .Include(r => r.User)
                .Include(r => r.Listing)
                .Where(r => r.ListingId == listingId)
                .OrderByDescending(r => r.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Report>> GetReportsByUserId(Guid userId, int pageIndex, int pageSize)
        {
            return await _context.Report
                .Include(r => r.User)
                .Include(r => r.Listing)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
