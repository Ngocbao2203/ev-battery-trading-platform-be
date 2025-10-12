using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Service.DTOs.Brand;
using EBTP.Service.DTOs.Listing;
using EBTP.Service.DTOs.ListingImage;
using EBTP.Service.DTOs.Package;
using EBTP.Service.DTOs.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //User
            CreateMap<User, UserDTO>().ReverseMap();

            //Brand 
            CreateMap<CreateBrandDTO, Brand>().ReverseMap();
            CreateMap<UpdateBrandDTO, Brand>().ReverseMap();
            CreateMap<BrandDTO, Brand>().ReverseMap();

            //Package
            CreateMap<CreatePakageDTO, Package>().ReverseMap();
            CreateMap<UpdatePackageDTO, Package>().ReverseMap();
            CreateMap<PackageDTO, Package>().ReverseMap();

            //Listing
            CreateMap<ListingDTO, Listing>()
                .ReverseMap();
            CreateMap<CreateListingDTO, Listing>()
                .ForMember(dest => dest.ListingImages, opt => opt.Ignore())
                .ReverseMap();

            //ListingImage
            CreateMap<ListingImageDTO, ListingImage>().ReverseMap();
            CreateMap<IFormFile, ListingImage>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }
    }
}
