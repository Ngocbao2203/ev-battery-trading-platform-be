using EBTP.Repository.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Listing
{
    public class CreateListingDTO
    {
        public CategoryEnum Category { get; set; }
        [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tiêu đề phải ít hơn 50 ký tự.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Giá phải lớn hơn bằng 0.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Tình trạng là bắt buộc.")]
        public ListingStatusEnum ListingStatus { get; set; }
        [Required(ErrorMessage = "Thương hiệu là bắt buộc.")]
        public Guid BrandId { get; set; }
        [Required(ErrorMessage = "Gói là bắt buộc.")]
        public Guid? PackageId { get; set; }
        public string Model { get; set; }
        public int YearOfManufacture { get; set; }
        [Required(ErrorMessage = "Khu vực là bắt buộc.")]
        public string Area { get; set; }
        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [MaxLength(1500, ErrorMessage = "Tiêu đề phải ít hơn 1500 ký tự.")]
        public string Description { get; set; }
        public int Odo { get; set; }
        public int BatteryCapacity { get; set; }
        public int ActualOperatingRange { get; set; }
        public int ChargingTime { get; set; }
        public string Color { get; set; }
        public int Size { get; set; }
        public int Mass { get; set; }
        public List<IFormFile> ListingImages { get; set; }
    }
}
