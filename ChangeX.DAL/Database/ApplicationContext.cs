using Microsoft.EntityFrameworkCore;
using ChangeX.DAL.Entities;

namespace ChangeX.DAL.Database
{
    public class ApplicationContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<CR> CRs { get; set; }
        public DbSet<CRStatus> CRStatues { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)  To Cancel Delete 
        //{
        //    foreach (var relationship in modelBuilder.Model.GetEntityTypes()
        //        .SelectMany(e => e.GetForeignKeys()))
        //    {
        //        relationship.DeleteBehavior = DeleteBehavior.NoAction;
        //    }
        //}
    }
}
