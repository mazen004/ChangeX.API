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
        public DbSet<Client> Clients { get; set; }
        public DbSet<CR> CRs { get; set; }
        public DbSet<CRStatus> CRStatues { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
