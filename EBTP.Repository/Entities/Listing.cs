using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class Listing : BaseEntity
    {
        public CategoryEnum Category { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public ListingStatusEnum ListingStatus { get; set; }
        public Guid BrandId { get; set; }
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
        public ICollection<ListingImage> ListingImages { get; set; } = new List<ListingImage>();
        public StatusEnum Status { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
    }
}
