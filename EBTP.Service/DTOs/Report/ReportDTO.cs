using EBTP.Repository.Enum;
using EBTP.Service.DTOs.ReportImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Report
{
    public class ReportDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ListingId { get; set; }
        public List<ReportImageDTO?> ReportImages { get; set; }
        public ReportReasonEnum Reason { get; set; }
        public string? OtherReason { get; set; }
    }
}
