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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Payment?> GetByTransactionNoAsync(string transactionNo)
        {
            return await _context.Payment
                .FirstOrDefaultAsync(p => p.TransactionNo == transactionNo);
        }
    }
}
