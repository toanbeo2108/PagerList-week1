using Microsoft.EntityFrameworkCore;

namespace Thực_tập_tuần_1.Models
{
    public class WorkDbContext : DbContext
    {
        public WorkDbContext()
        {

        }
        public WorkDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Work> Works { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-QQ18T65\SQLEXPRESS;Database=ABCSOFT_Week1;Trusted_Connection=True;");
        }
    }
}
