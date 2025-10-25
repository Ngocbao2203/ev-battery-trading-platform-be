using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Package
{
    public class CreatePakageDTO
    {
        [Required(ErrorMessage = "Tên gói là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên gói phải ít hơn 50 ký tự.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Giá gói là bắt buộc.")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Giá phải lớn hơn bằng 0.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Thời gian hiệu lực của gói là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Thời gian hiệu lực của gói phải lớn hơn 0.")]
        public int DurationInDays { get; set; }
        [Required(ErrorMessage = "Thông tin gói là bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Thông tin gói phải ít hơn 255 ký tự.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Loại gói là bắt buộc.")]
        public PackageTypeEnum PackageType { get; set; }
        [Required(ErrorMessage = "Trạng thái gói là bắt buộc.")]
        public StatusEnum Status { get; set; }
    }
}
