using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ListingId { get; set; }
        public Guid PackageId { get; set; }
        public Guid? PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Notes { get; set; }
        public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Pending;
        public string PaymentMethod { get; set; }

        [ForeignKey("ListingId")]
        public Listing Listing { get; set; } = null!;
        [ForeignKey("PackageId")]
        public Package Package { get; set; } = null!;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        public Payment? Payment { get; set; }
    }
}
