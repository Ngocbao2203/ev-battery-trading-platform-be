using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Favourite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IFavouriteService
    {
        public Task<Result<List<ViewFavouriteDTO>>> GetFavourites(Guid id);
        public Task<Result<object>> AddFavourite(CreateFavouriteDTO request);
        public Task<Result<object>> DeleteFavouriteByUserAndListing(Guid userId, Guid listingId);
        public Task<Result<bool>> CheckFavourite(Guid userId, Guid listingId);
    }
}
