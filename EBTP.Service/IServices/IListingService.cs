using EBTP.Repository.Enum;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Listing;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IListingService
    {
        Task<Result<List<ListingDTO>>> GetAllAsync(string? search, int pageIndex, int pageSize, decimal? from, decimal? to, ListingStatusEnum? listingStatusEnum, CategoryEnum? categoryEnum);
        Task<Result<List<ListingDTO>>> GetListingsByStatusAsync(int pageIndex, int pageSize, decimal? from, decimal? to, StatusEnum? status);
        Task<Result<ListingDTO>> GetByIdAsync(Guid id);
        Task<Result<List<ListingDTO>>> GetMyListingsAsync(int pageIndex, int pageSize);
        Task<Result<object>> CreateAsync(CreateListingDTO createListingDTO);
        /*        Task<Result<ListingDTO>> UpdateAsync(Guid id, UpdateListingDTO updateListingDTO);
                Task<Result<bool>> DeleteAsync(Guid id);*/
        Task<Result<string>> CreateVnPayUrlAsync(Guid listingId, HttpContext httpContext);
        Task<Result<string>> RetryPayment(Guid listingId, HttpContext httpContext);
        Task<Result<object>> HandleVnPayReturnAsync(IQueryCollection query);

        //Admin
        Task<Result<object>> AcceptListingAsync(Guid listingId);
        Task<Result<object>> RejectListingAsync(Guid listingId, string? reason);
        Task<Result<ListingDTO>> UpdateAsync(UpdateListingDTO updateListingDTO);
        
        //Hangfire
        Task AutoChangeStatusWhenListingExpiredAsync();
    }
}