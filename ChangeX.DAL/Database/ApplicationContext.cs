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
            modelBuilder.Entity<CRStatus>().HasData(
               new CRStatus
               {
                   ID = Guid.Parse("3F2A9E7D-8B41-4C6A-9D2E-1A7F5C8B3E90"),
                   CurrentStatus = "Pending Vendor FeedBack",
                   AvailableStatusIDs = "2E7C9A4D-5F3B-4C1E-8D6A-7B9F2C4E1A85,8A3E6C1F-4B9D-4E2A-9F7C-2D5B8E4A1C96,7C1D4E2F-9A6B-4F3D-8E7C-2B9A5D1F6C43",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("7C1D4E2F-9A6B-4F3D-8E7C-2B9A5D1F6C43"),
                   CurrentStatus = "Pending Client Clarification",
                   AvailableStatusIDs = "3F2A9E7D-8B41-4C6A-9D2E-1A7F5C8B3E90",
                   AccessedBy = "Client"
               },
               new CRStatus
               {
                   ID = Guid.Parse("2E7C9A4D-5F3B-4C1E-8D6A-7B9F2C4E1A85"),
                   CurrentStatus = "Accepted (CR)",
                   AvailableStatusIDs = "6F4B2E8D-1A9C-4D7F-B3E6-8C2A5F9D4B17",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("6F4B2E8D-1A9C-4D7F-B3E6-8C2A5F9D4B17"),
                   CurrentStatus = "Estimation Created",
                   AvailableStatusIDs = "3F2A9E7D-8B41-4C6A-9D2E-1A7F5C8B3E90",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("A5E9C3B7-2D4F-4A8E-9C1B-6F3D7E2A9B58"),
                   CurrentStatus = "Pending Client Approval",
                   AvailableStatusIDs = "9D3F6A2E-4C8B-4F1D-A7E9-2B6C4D8A3F71,8A3E6C1F-4B9D-4E2A-9F7C-2D5B8E4A1C96",
                   AccessedBy = "Client"
               },
               new CRStatus
               {
                   ID = Guid.Parse("9D3F6A2E-4C8B-4F1D-A7E9-2B6C4D8A3F71"),
                   CurrentStatus = "Accepted (Estimation)",
                   AvailableStatusIDs = "1C8E4B7A-3D9F-4E2C-B6A8-5F3D9E1C7A42",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("1C8E4B7A-3D9F-4E2C-B6A8-5F3D9E1C7A42"),
                   CurrentStatus = "Analysis",
                   AvailableStatusIDs = "6A4D2F9E-8C3B-4A7D-9E1F-4B8A6D2C5F93,A5E9C3B7-2D4F-4A8E-9C1B-6F3D7E2A9B58",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("6A4D2F9E-8C3B-4A7D-9E1F-4B8A6D2C5F93"),
                   CurrentStatus = "Design",
                   AvailableStatusIDs = "F3B9E2D4-7A6C-4D8E-B2F1-9C5A3E7D4B26",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("F3B9E2D4-7A6C-4D8E-B2F1-9C5A3E7D4B26"),
                   CurrentStatus = "Development",
                   AvailableStatusIDs = "E2B7A4C9-6F1D-4E3A-8B9C-3D5A7F2E1C64",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("E2B7A4C9-6F1D-4E3A-8B9C-3D5A7F2E1C64"),
                   CurrentStatus = "Testing",
                   AvailableStatusIDs = "8D4F2C6E-3A9B-4E7D-9C1F-5A2D9B6C3E47",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("8D4F2C6E-3A9B-4E7D-9C1F-5A2D9B6C3E47"),
                   CurrentStatus = "Pending Customer Approval",
                   AvailableStatusIDs = "B7E3A9C4-2F8D-4B6E-9A1C-6D4F2E8A7C53,8A3E6C1F-4B9D-4E2A-9F7C-2D5B8E4A1C96,4B9E7C2A-6D3F-4A8E-9C2B-1E7A4D8C6F39",
                   AccessedBy = "Client"
               },
               new CRStatus
               {
                   ID = Guid.Parse("4B9E7C2A-6D3F-4A8E-9C2B-1E7A4D8C6F39"),
                   CurrentStatus = "Rework Required",
                   AvailableStatusIDs = "1C8E4B7A-3D9F-4E2C-B6A8-5F3D9E1C7A42",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("B7E3A9C4-2F8D-4B6E-9A1C-6D4F2E8A7C53"),
                   CurrentStatus = "Accepted (Test)",
                   AvailableStatusIDs = "D9A4C2F7-6E3B-4D8A-B7C1-2F9E5A3D8C64",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("D9A4C2F7-6E3B-4D8A-B7C1-2F9E5A3D8C64"),
                   CurrentStatus = "Deployed",
                   AvailableStatusIDs = "5C2E8A4D-9F7B-4E1C-A3D6-8B4F2C9E7A15",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("5C2E8A4D-9F7B-4E1C-A3D6-8B4F2C9E7A15"),
                   CurrentStatus = "Delivered",
                   AvailableStatusIDs = "C1F7A4E9-8B2D-4E6C-A3F1-7C9E2A5D8B64",
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("8A3E6C1F-4B9D-4E2A-9F7C-2D5B8E4A1C96"),
                   CurrentStatus = "Rejected",
                   AvailableStatusIDs = null,
                   AccessedBy = "Admin"
               },
               new CRStatus
               {
                   ID = Guid.Parse("C1F7A4E9-8B2D-4E6C-A3F1-7C9E2A5D8B64"),
                   CurrentStatus = "Completed",
                   AvailableStatusIDs = null,
                   AccessedBy = "Admin"
               }
           );
        }
    }
}
