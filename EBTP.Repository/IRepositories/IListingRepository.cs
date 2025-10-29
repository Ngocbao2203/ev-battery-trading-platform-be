using EBTP.Repository.Entities;
using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IListingRepository : IGenericRepository<Listing>
    {
        Task<List<Listing>> GetAllListings(string? search, int pageIndex, int pageSize, decimal? from, decimal? to, ListingStatusEnum? listingStatusEnum, CategoryEnum? categoryEnum);
        Task<List<Listing>> GetListingsByStatus(int pageIndex, int pageSize, decimal? from, decimal? to, StatusEnum? status);
        Task<Listing> GetListingById(Guid id);
        Task<List<Listing>> GetListingsByUserId(Guid userId, int pageIndex, int pageSize);
        Task<bool> CheckIsFirstListing(Guid userId);
        Task<List<Listing>> CheckListingExpired();
    }
}