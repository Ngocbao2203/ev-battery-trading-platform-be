using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Dashboard
{
    public class ListingDashboard
    {
        public int TotalListings { get; set; }
        public int TotalOldCars { get; set; }
        public int TotalNewCars { get; set; }
        public int TotalOldBikes { get; set; }
        public int TotalNewBikes { get; set; }
        public int TotalOldBateries { get; set; }
        public int TotalNewBateries { get; set; }
    }
}
