using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Service.DTOs.Brand;
using EBTP.Service.DTOs.ChatThread;
using EBTP.Service.DTOs.Favourite;
using EBTP.Service.DTOs.Listing;
using EBTP.Service.DTOs.ListingImage;
using EBTP.Service.DTOs.Message;
using EBTP.Service.DTOs.Package;
using EBTP.Service.DTOs.Report;
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
            CreateMap<Listing, ViewListingFavouriteDTO>()
                .ForMember(dest => dest.ListingImages, opt => opt.MapFrom(src => src.ListingImages))
                .ReverseMap();

            //ListingImage
            CreateMap<ListingImageDTO, ListingImage>().ReverseMap();
            CreateMap<IFormFile, ListingImage>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            //Favourite
            CreateMap<Favourite, CreateFavouriteDTO>().ReverseMap();
            CreateMap<ViewFavouriteDTO, Favourite>().ReverseMap();

            //Report
            CreateMap<Report, ReportDTO>().ReverseMap();
            CreateMap<Report, CreateReportDTO>().ReverseMap();

            //Message
            CreateMap<SendMessageDTO, Message>().ReverseMap();
            CreateMap<ViewMessageDTO, Message>().ReverseMap()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id));

            //CharThread
            CreateMap<ViewChatThreadDTO, ChatThread>().ReverseMap()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id));
            CreateMap<ChatThreadDTO, ChatThread>().ReverseMap();
            CreateMap<ChatThread, CreateChatThreadDTO>()
            .ReverseMap();
        }
    }
}
