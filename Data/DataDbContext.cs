using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> opts) : base(opts)
        {

        }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<CountryModel> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookModel>().HasOne(b => b.Country).WithMany(c => c.Books).HasForeignKey(f => f.CountryId);
            modelBuilder.Entity<CountryModel>().HasMany(c => c.Books).WithOne(b => b.Country).HasForeignKey(f => f.CountryId);
        }
    }
}
