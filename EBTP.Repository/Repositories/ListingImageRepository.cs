using EBTP.Repository.Data;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Repositories
{
    public class ListingImageRepository : GenericRepository<ListingImage>, IListingImageRepository
    {
        public ListingImageRepository(AppDbContext context) : base(context)
        {
        }

    }
}
