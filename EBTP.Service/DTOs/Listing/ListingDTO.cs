using EBTP.Repository.Enum;
using EBTP.Service.DTOs.Brand;
using EBTP.Service.DTOs.ListingImage;
using EBTP.Service.DTOs.Package;
using EBTP.Service.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Listing
{
    public class ListingDTO
    {
        public Guid Id { get; set; }
        public CategoryEnum Category { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public ListingStatusEnum ListingStatus { get; set; }
        public string Model { get; set; }
        public int YearOfManufacture { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public int Odo { get; set; }
        public int BatteryCapacity { get; set; }
        public int ActualOperatingRange { get; set; }
        public int ChargingTime { get; set; }
        public string Color { get; set; }
        public int Size { get; set; }
        public int Mass { get; set; }
        public List<ListingImageDTO?> ListingImages { get; set; }
        public UserDTO User { get; set; }
        public BrandDTO Brand { get; set; }
        public PackageDTO Package { get; set; }
    }
}
