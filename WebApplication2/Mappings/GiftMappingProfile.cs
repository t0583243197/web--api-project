using AutoMapper; // מייבא AutoMapper
using WebApplication2.Models; // מייבא מודלים
using WebApplication2.Models.DTO; // מייבא DTOs

namespace WebApplication2.Mappings // מרחב למיפויים
{ // התחלת namespace
    public class GiftMappingProfile : Profile // פרופיל מיפוי ל-AutoMapper
    { // התחלת מחלקה
        public GiftMappingProfile() // בנאי המגדיר מפות
        { // התחלת בנאי
            // --- מיפוי מתנות ---
            CreateMap<GiftModel, GiftDTO>(); // מיפוי ממודל ל-DTO

            CreateMap<GiftDTO, GiftModel>() // מיפוי מה-DTO חזרה למודל
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // התעלם מה-Id בעדכון

            // --- מיפוי תורמים ---
            CreateMap<DonorModel, donorDTO>().ReverseMap(); // מיפוי דו-כיווני לתורם

            // --- מיפוי קטגוריות ---
            CreateMap<CategoryModel, CategoryDTO>().ReverseMap(); // מיפוי דו-כיווני לקטגוריה

            // --- מיפוי משתמשים ---
            CreateMap<UserDto, UserModel>().ReverseMap(); // מיפוי דו-כיווני למשתמש

            CreateMap<UserDto, UserModel>().ReverseMap(); // שורה כפולה (ניתן להסיר כפילות)
        } // סיום בנאי
    } // סיום מחלקה
} // סיום namespace