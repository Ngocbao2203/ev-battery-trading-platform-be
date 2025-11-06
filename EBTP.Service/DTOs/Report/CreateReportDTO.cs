using EBTP.Repository.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Report
{
    public class CreateReportDTO
    {
        public Guid UserId { get; set; }
        public Guid ListingId { get; set; }
        public ReportReasonEnum Reason { get; set; }
        public string? OtherReason { get; set; }
        public IFormFile ImageReport { get; set; }
    }
}
