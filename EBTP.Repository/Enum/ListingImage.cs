using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Enum
{
    public class ListingImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public Guid ListingId { get; set; }
        [ForeignKey("ListingId")]
        public Listing Listing { get; set; }
    }
}
