using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IFavouriteRepository : IGenericRepository<Favourite>
    {
        public Task<List<Favourite>> GetFavouritesByUserId(Guid userId);
        public Task AddFavourite(Favourite favourite);
        public Task<Favourite> GetFavouriteByUserAndListing(Guid userId, Guid listingId);
        public Task DeleteFavouriteByUserAndListing(Guid userId, Guid listingId);
        public Task<bool> CheckFavourite(Guid userId, Guid listingId);
    }
}
