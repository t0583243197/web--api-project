using AutoMapper; // מייבא AutoMapper
using WebApplication2.Models; // מייבא מודלים
using WebApplication2.Models.DTO; // מייבא DTOs
using System.Linq;

namespace WebApplication2.Mappings
{
    public class GiftMappingProfile : Profile
    {
        public GiftMappingProfile()
        {
            // --- מיפוי מתנות ---
            CreateMap<GiftDTO, GiftModel>()
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.TicketPrice))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Donor, opt => opt.Ignore());

            CreateMap<GiftModel, GiftDTO>()
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.TicketPrice))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src => src.Donor != null ? src.Donor.Name : null));

            // --- מיפוי תורמים: let AutoMapper map the collection directly (no context.Mapper in expression) ---
            CreateMap<DonorModel, DonorDTO>()
                .ForMember(dest => dest.Gifts, opt => opt.MapFrom(src => src.Gifts));
            CreateMap<DonorDTO, DonorModel>();

            // --- מיפוי קטגוריות ---
            CreateMap<CategoryModel, CategoryDTO>().ReverseMap();
            CreateMap<WinnerModel, WinnerDTO>().ReverseMap();
            // --- מיפוי משתמשים ---
            CreateMap<UserDto, UserModel>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role ?? "Customer")));

            CreateMap<UserModel, UserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<OrderTicketModel, OrderItemDTO>();
            CreateMap<OrderModel, OrderDTO>();
            CreateMap<TicketModel, TicketDTO>();
            CreateMap<TicketDTO, TicketModel>();
            CreateMap<OrderModel, OrderTicketModel>();

        }
    }
}