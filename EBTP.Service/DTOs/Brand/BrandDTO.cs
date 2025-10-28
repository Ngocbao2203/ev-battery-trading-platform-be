using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Brand
{
    public class BrandDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BrandTypeEnum Type { get; set; }
    }
}
