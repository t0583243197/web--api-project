namespace WebApplication2.Models
{
    public class CategoryModel
    {
      
            public int Id { get; set; } // מזהה קטגוריה [cite: 45]
            public string Name { get; set; } // שם הקטגוריה (למשל: ריהוט, נופש) 
            public List<GiftModel> Gifts { get; set; } // רשימת המתנות המשויכות לקטגוריה זו 
       
    }
}
