using Microsoft.EntityFrameworkCore;
using ChangeX.DAL.Entities;

namespace ChangeX.DAL.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
        }
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<CR> CRs => Set<CR>();
        public DbSet<CRStatus> CRStatues => Set<CRStatus>();
        public DbSet<Detail> Details => Set<Detail>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
