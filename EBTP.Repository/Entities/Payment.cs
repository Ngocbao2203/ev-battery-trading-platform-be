using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class Payment : BaseEntity
    {
        public string? TransactionNo { get; set; }
        public string? BankCode { get; set; }
        public string? ResponseCode { get; set; }
        public string? SecureHash { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? RawData { get; set; }
        public bool IsRefunded { get; set; }
        public Guid? ListingId { get; set; }
        public Listing? Listing { get; set; }
        public bool IsSuccess => ResponseCode == "00";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
