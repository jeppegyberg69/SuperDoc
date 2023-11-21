
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Contexts
{
    public class SuperDocContext : DbContext
    {
        public DbSet<User> Users { get => Set<User>(); }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public SuperDocContext(DbContextOptions<SuperDocContext> contextOptions) : base(contextOptions)
        {
        }
    }

    public class DatabaseContextFactory : IDesignTimeDbContextFactory<SuperDocContext>
    {
        // Only used at design time (e.g. migrations)
        public SuperDocContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SuperDocContext> optionsBuilder = new DbContextOptionsBuilder<SuperDocContext>();
            optionsBuilder.UseSqlServer(@"Server=Snowstorm1.myqnapcloud.com;Database=SuperDoc;TrustServerCertificate=True;Encrypt=True;User Id=sa;Password=dYPmwpghUwlhE0297jTYXGIAhHF9;");


            return new SuperDocContext(optionsBuilder.Options);
        }
    }


}
