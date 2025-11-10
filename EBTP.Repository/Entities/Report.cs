using EBTP.Repository.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    [Index(nameof(UserId), nameof(ListingId), IsUnique = true)]
    public class Report : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ListingId { get; set; }
        public ReportReasonEnum Reason { get; set; }
        public string? OtherReason { get; set; }
        public User User { get; set; }
        public Listing Listing { get; set; }
        public ICollection<ReportImage> ReportImages { get; set; } = new List<ReportImage>();
    }
}
