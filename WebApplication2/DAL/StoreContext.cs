using System.Net.Sockets;
using Microsoft.EntityFrameworkCore; // מייבא EF Core
using WebApplication2.Models; // מייבא מודלים

namespace WebApplication2.DAL // מרחב שמות ל-DAL
{ // התחלת namespace
    public class StoreContext : DbContext // DbContext של האפליקציה
    { // התחלת מחלקה
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { } // בנאי המקבל אופציות

        public DbSet<UserModel> Users { get; set; } // DbSet למשתמשים
        public DbSet<DonorModel> Donors { get; set; } // DbSet לתורמים
        public DbSet<GiftModel> Gifts { get; set; } // DbSet למתנות
        public DbSet<CategoryModel> Categories { get; set; } // DbSet לקטגוריות

        public DbSet<OrderModel> Orders { get; set; } // DbSet להזמנות

        public DbSet<OrderTicketModel> OrderTicket { get; set; } // DbSet לפרטי הזמנה

        public DbSet<WinnerModel> Winners { get; set; } // DbSet לזוכים
        public DbSet<TicketModel> Tickets { get; set; } // DbSet לכרטיסים
        protected override void OnModelCreating(ModelBuilder modelBuilder) // קונפיגורציה של מודלים
        { // התחלת שיטה
          // הגדרת מפתח ראשי
            modelBuilder.Entity<GiftModel>().HasKey(g => g.Id);
            modelBuilder.Entity<GiftModel>() // קונפיגורציית GiftModel
                .Property(g => g.TicketPrice) // בחירת שדה TicketPrice
                .HasColumnType("decimal(18,2)"); // הגדרת סוג עמודה

            modelBuilder.Entity<GiftModel>() // קשר Gift -> Donor
                .HasOne(g => g.Donor) // לכל מתנה יש תורם
                .WithMany(d => d.Gifts) // לתורם יכולים להיות רבות מתנות
                .HasForeignKey(g => g.DonorId); // העמודה שמקשרת ביניהם

            modelBuilder.Entity<GiftModel>() // קשר Gift -> Category
                .HasOne(g => g.Category) // לכל מתנה יש קטגוריה
                .WithMany(c => c.Gifts) // לקטגוריה יכולות להיות רבות מתנות
                .HasForeignKey(g => g.CategoryId); // העמודה שמקשרת ביניהם

            modelBuilder.Entity<OrderTicketModel>() // קשר OrderItem -> Order
                .HasOne(oi => oi.Order) // לכל פריט יש הזמנה
                .WithMany(o => o.OrderItems) // להזמנה יש פריטים רבים
                .HasForeignKey(oi => oi.OrderId); // מפתח זר

            base.OnModelCreating(modelBuilder); // קריאה למימוש הבסיסי

            modelBuilder.Entity<WinnerModel>().HasKey(w => w.Id);

            // קשר זוכה -> מתנה (1 ל-1 או 1 ל-רבים תלוי בלוגיקה, כאן נגדיר שלכל זכייה יש מתנה אחת)
            modelBuilder.Entity<WinnerModel>()
                .HasOne(w => w.Gift) // אובייקט המתנה בתוך מודל הזוכה
                .WithMany()            // למתנה יכולים להיות מספר זוכים (למשל בהגרלות שונות)
                .HasForeignKey(w => w.GiftId)
                .OnDelete(DeleteBehavior.Restrict); // מניעת מחיקה משורשרת כדי לשמור על היסטוריה

            // קשר זוכה -> משתמש/רוכש
            modelBuilder.Entity<WinnerModel>()
                .HasOne(w => w.User)   // המשתמש שזכה
                .WithMany()            // למשתמש יכולות להיות מספר זכיות
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        
    } // סיום שיטה
    } // סיום מחלקה
} // סיום namespace