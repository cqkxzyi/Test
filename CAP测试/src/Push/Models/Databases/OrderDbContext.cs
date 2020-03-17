using Microsoft.EntityFrameworkCore;

namespace Push.Models
{
    public class OrderDbContext : DbContext
    {
        public string ConnStr { get; }

        public OrderDbContext(DbContextOptions options, string connStr) : base(options)
        {
            ConnStr = connStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnStr);
        }

        /// <summary>
        /// 数据表注册
        /// </summary>
        public DbSet<Orders> Orders { get; set; }
    }
}
