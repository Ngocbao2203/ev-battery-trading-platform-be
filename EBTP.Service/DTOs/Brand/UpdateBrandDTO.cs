using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Brand
{
    public class UpdateBrandDTO
    {
        [Required(ErrorMessage = "Mã hãng là bắt buộc.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Tên hãng là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên hãng phải ít hơn 50 ký tự.")]
        public string Name { get; set; }
    }
}
