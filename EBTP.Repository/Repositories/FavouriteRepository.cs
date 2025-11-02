using EBTP.Repository.Data;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Repositories
{
    public class FavouriteRepository : GenericRepository<Favourite>, IFavouriteRepository
    {
        private readonly AppDbContext _context;
        public FavouriteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddFavourite(Favourite favourite)
        {
            await _context.Favourite.AddAsync(favourite);
        }

        public async Task<Favourite> GetFavouriteByUserAndListing(Guid userId, Guid listingId)
        {
            return await _context.Favourite
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ListingId == listingId);
        }

        public async Task DeleteFavouriteByUserAndListing(Guid userId, Guid listingId)
        {
            var favourite = await GetFavouriteByUserAndListing(userId, listingId);
            if (favourite != null)
            {
                _context.Favourite.Remove(favourite);
            }
        }

        public async Task<List<Favourite>> GetFavouritesByUserId(Guid id)
        {
            return await _context.Favourite
                .Include(fa => fa.User)
                .Include(fa => fa.Listing)
                .ThenInclude(fa => fa.ListingImages)
                .Where(fa => fa.UserId == id)
                .ToListAsync();
        }

        public async Task<bool> CheckFavourite(Guid userId, Guid listingId)
        {
            return await _context.Favourite
                .AnyAsync(f => f.UserId == userId && f.ListingId == listingId);
        }
    }
}
