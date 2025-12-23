namespace WebApplication2.Models.DTO
{
    public class CategoryDTO
    {
      
            // מזהה ייחודי של הקטגוריה - נחוץ כדי לשלוח לשרת בזמן סינון או הוספת מתנה
            public int Id { get; set; }

            // שם הקטגוריה (למשל: "חשמל", "ריהוט", "נופש") - מוצג למשתמש בממשק
            public string Name { get; set; }
        
    }
}
