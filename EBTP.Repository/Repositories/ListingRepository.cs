using EBTP.Repository.Data;
using EBTP.Repository.Entities;
using EBTP.Repository.Enum;
using EBTP.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Repositories
{
    public class ListingRepository : GenericRepository<Listing>, IListingRepository
    {
        private readonly AppDbContext _context;
        public ListingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Listing>> GetAllListings(string? search, int pageIndex, int pageSize, decimal? from, decimal? to, ListingStatusEnum? listingStatusEnum, CategoryEnum? categoryEnum)
        {
            var query = _context.Listing.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Title.Contains(search));
            }

            if (from.HasValue)
            {
                query = query.Where(x => x.Price >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(x => x.Price <= to.Value);
            }
            if (listingStatusEnum.HasValue)
            {
                query = query.Where(x => x.ListingStatus == listingStatusEnum.Value);
            }

            if (categoryEnum.HasValue)
            {
                query = query.Where(x => x.Category == categoryEnum.Value);
            }

            return await query
     .Include(lt => lt.ListingImages)
     .Include(lt => lt.Brand)
     .Include(lt => lt.User)
     .Include(lt => lt.Package)
     .Where(x => x.Status == StatusEnum.Active && !x.IsDeleted)
     .OrderByDescending(x => x.CreationDate)
     .Skip((pageIndex - 1) * pageSize)
     .Take(pageSize)
     .ToListAsync();
        }

        public async Task<List<Listing>> GetListingsByStatus(int pageIndex, int pageSize, decimal? from, decimal? to, StatusEnum? status)
        {
             var query = _context.Listing.AsQueryable();

            if (from.HasValue)
            {
                query = query.Where(x => x.Price >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(x => x.Price <= to.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            return await query
                .Include(lt => lt.ListingImages)
                .Include(lt => lt.Brand)
                .Include(lt => lt.User)
                .Include(lt => lt.Package)
                .OrderByDescending(x => x.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Listing> GetListingById(Guid id)
        {
            var listing = await _context.Listing
                .Include(lt => lt.ListingImages)
                .Include(lt => lt.Brand)
                .Include(lt => lt.User)
                .Include(lt => lt.Package)
                .FirstOrDefaultAsync(x => x.Id == id);

            return listing;
        }

        public async Task<List<Listing>> GetListingsByUserId(Guid userId, int pageIndex, int pageSize)
        {
            var query = _context.Listing
                .Where(x => x.UserId == userId && !x.IsDeleted) // Chỉ filter theo userId và IsDeleted
                .Include(lt => lt.ListingImages)
                .Include(lt => lt.Brand)
                .Include(lt => lt.User)
                .Include(lt => lt.Package);

            return await query
                .OrderByDescending(x => x.CreationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}