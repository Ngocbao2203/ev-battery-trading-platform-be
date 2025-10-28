using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Brand
{
    public class CreateBrandDTO
    {
        [Required(ErrorMessage = "Tên hãng là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên hãng phải ít hơn 50 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Loại phương tiện là bắt buộc.")]
        public BrandTypeEnum Type { get; set; }
    }
}
