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
          
            // DTO -> Model
            // חשוב: ב־GiftDTO שדה Category הוא מחרוזת (string), לכן לא ניתן לקרוא src.Category.Name.
            // כדי לא לייצר שגיאות ושרשראות, נוותר על מיפוי ישיר של הניווטים כאן ונמפה רק שדות פשוטים.
            CreateMap<GiftDTO, GiftModel>()
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.TicketPrice))
                // אל תנסו למxxx שדות ניווט/אובייקטיים ישירות מכיוון ש־GiftDTO.Category הוא string.
                // השארת Category ו־Donor להתנהלות ידנית (BLL/EF) מונעת circular references ושגיאות קומפילציה.
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Donor, opt => opt.Ignore());

            // Model -> DTO
            // כאן מומלץ למxxx את שדה ה־Category ל־string ב־DTO בעזרת src.Category.Name (כשהניווט קיים).
            CreateMap<GiftModel, GiftDTO>()
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.TicketPrice))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src => src.Donor != null ? src.Donor.Name : null));

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
            CreateMap<WinnerModel, WinnerDTO>().ReverseMap();
            // --- מיפוי משתמשים ---
            CreateMap<UserDto, UserModel>().ReverseMap();
            
            CreateMap<OrderTicketModel, OrderItemDTO>();
            CreateMap<OrderModel, OrderDTO>();
            CreateMap<TicketModel, TicketDTO>();
            CreateMap<TicketDTO, TicketModel>();
        } 
        // סיום בנאי
    } // סיום מחלקה
} // סיום namespace