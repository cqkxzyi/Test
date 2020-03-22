using Microsoft.EntityFrameworkCore;

namespace Delivery.Models
{
    public class DeliveryDbContext : DbContext
    {
        public string ConnStr { get; }

        public DeliveryDbContext(DbContextOptions options, string connStr) : base(options)
        {
            ConnStr = connStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnStr);
        }

        public DbSet<Delivery> Deliveries { get; set; }
    }
}
