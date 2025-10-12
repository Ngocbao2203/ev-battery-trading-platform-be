using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class Package : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public string Description { get; set; }
        public PackageTypeEnum PackageType { get; set; }
        public StatusEnum Status { get; set; }
    }
}
