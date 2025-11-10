using EBTP.Repository.Enum;
using EBTP.Service.DTOs.Listing;
using EBTP.Service.DTOs.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Transaction
{
    public class TransactionDTO
    {
        public Guid UserId { get; set; }
        public Guid? PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Notes { get; set; }
        public PaymentStatusEnum Status { get; set; }
        public string? PaymentMethod { get; set; }
        public ListingDTO Listing { get; set; }
        public PackageDTO Package { get; set; }
    }
}
