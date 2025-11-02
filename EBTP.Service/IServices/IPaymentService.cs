using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentUrl(string transactionNo, decimal totalAmount, HttpContext context);
        Task<bool> ValidateReturnData(IQueryCollection query);
        Task<string> QueryTransactionAsync(string txnRef, string orderInfo, string transactionDate);

    }
}
