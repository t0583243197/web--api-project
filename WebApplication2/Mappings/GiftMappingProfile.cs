using AutoMapper; // מייבא AutoMapper
using WebApplication2.Models; // מייבא מודלים
using WebApplication2.Models.DTO; // מייבא DTOs
using System.Linq;

namespace WebApplication2.Mappings // מרחב למיפויים
{ // התחלת namespace
    public class GiftMappingProfile : Profile // פרופיל מיפוי ל-AutoMapper
    { // התחלת מחלקה
        public GiftMappingProfile() // בנאי המגדיר מפות
        { // התחלת בנאי
            // --- מיפוי מתנות ---
          
            CreateMap<GiftDTO, GiftModel>()
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.TicketPrice))
                // map or resolve Category; ignore for now if you set Category separately
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                // avoid setting navigation back to donor here (EF will set DonorId)
                .ForMember(dest => dest.Donor, opt => opt.Ignore());

            CreateMap<GiftModel, GiftDTO>();

            // --- מיפוי תורמים (תיקון רישיות) ---
            CreateMap<DonorModel, DonorDTO>()
                // ensure nested list mapping works
                .ForMember(dest => dest.Gifts, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Gifts != null
                        ? src.Gifts.Select(gift => context.Mapper.Map<GiftDTO>(gift)).ToList()
                        : new System.Collections.Generic.List<GiftDTO>()));
            CreateMap<DonorDTO, DonorModel>();

            // --- מיפוי קטגוריות ---
            CreateMap<CategoryModel, CategoryDTO>().ReverseMap();

            // --- מיפוי משתמשים ---
            CreateMap<UserDto, UserModel>().ReverseMap();

        } // סיום בנאי
    } // סיום מחלקה
} // סיום namespace