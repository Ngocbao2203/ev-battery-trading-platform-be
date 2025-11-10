using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface ITransactionService
    {
        Task<Result<List<TransactionDTO>>> GetTransactionsByUserId(Guid userId, int pageIndex, int pageSize);
    }
}
