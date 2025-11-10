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
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly AppDbContext _context;
        public TransactionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Transaction>> GetTransactionsByUserId(Guid userId, int pageIndex, int pageSize)
        {
            return await _context.Transaction
                .Include(t => t.User)
                .Include(t => t.Listing)
                .Include(t => t.Package)
                .Include(t => t.Payment)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
