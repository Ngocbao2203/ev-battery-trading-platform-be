using EBTP.Repository.Enum;
using EBTP.Service.DTOs.ListingImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Listing
{
    public class ViewListingFavouriteDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public StatusEnum Status { get; set; }
        public List<ListingImageDTO> ListingImages { get; set; } = new();
    }
}
