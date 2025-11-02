using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Favourite
{
    public class CreateFavouriteDTO
    {
        public Guid UserId { get; set; }
        public Guid ListingId { get; set; }
    }
}
