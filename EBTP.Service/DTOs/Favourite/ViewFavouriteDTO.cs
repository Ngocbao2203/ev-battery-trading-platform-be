using EBTP.Service.DTOs.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Favourite
{
    public class ViewFavouriteDTO
    {
        public Guid Id { get; set; }
        public ViewListingFavouriteDTO Listing { get; set; }
    }
}
