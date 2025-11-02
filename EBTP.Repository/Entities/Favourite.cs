using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class Favourite : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ListingId { get; set; }
        public User User { get; set; } = null!;
        public Listing Listing { get; set; } = null!;
    }
}
