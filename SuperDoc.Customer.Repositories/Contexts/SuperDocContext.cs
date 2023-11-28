using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Contexts
{
    public class SuperDocContext : DbContext
    {
        public DbSet<Case> Cases { get => Set<Case>(); }
        public DbSet<User> Users { get => Set<User>(); }
        public DbSet<Document> Documents { get => Set<Document>(); }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Case>().HasMany(c => c.CaseManagers).WithMany(u => u.Cases).UsingEntity(x => x.ToTable("CaseUsers"));

            builder.Entity<Document>().HasMany(d => d.ExternalUsers).WithMany(u => u.Documents).UsingEntity(x => x.ToTable("DocumentExternalUsers"));

            builder.Entity<Case>().HasOne(c => c.ResponsibleUser).WithMany(u => u.ResonsibleCases).OnDelete(DeleteBehavior.SetNull);
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
