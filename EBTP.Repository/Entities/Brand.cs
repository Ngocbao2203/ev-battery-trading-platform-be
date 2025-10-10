using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Listing> Listings { get; set; }
    }
}
