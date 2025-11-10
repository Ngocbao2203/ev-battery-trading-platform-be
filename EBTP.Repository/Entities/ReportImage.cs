using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class ReportImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public Guid ReportId { get; set; }
        [ForeignKey("ReportId")]
        public Report Report { get; set; }
    }
}
