using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public class StoreContext : DbContext
    {
        public DbSet<GiftModel> Gifts => Set<GiftModel>();
        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<TicketModel> Tickets => Set<TicketModel>();

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
    }
}
