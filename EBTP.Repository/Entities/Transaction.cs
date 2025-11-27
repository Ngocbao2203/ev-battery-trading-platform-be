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
        public Guid? UserId { get; set; }
        public Guid ListingId { get; set; }
        public Guid PackageId { get; set; }
        public Guid? PaymentId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public string? Notes { get; set; }
        public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Pending;
        public PaymentTypeEnum PaymentTypeEnum { get; set; }
       
        public string? PaymentMethod { get; set; }

        [ForeignKey(nameof(ListingId))]
        public Listing Listing { get; set; } = null!;

        [ForeignKey(nameof(PackageId))]
        public Package Package { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(PaymentId))]
        public Payment? Payment { get; set; }
    }
}
