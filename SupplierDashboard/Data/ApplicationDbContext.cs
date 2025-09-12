using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Models;
using SupplierDashboard.Models.Entities;

namespace SupplierDashboard.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<SubAgency> SubAgencies { get; set; }
        public DbSet<Markup> Markups { get; set; }
        public DbSet<MarkupAgency> MarkupAgencies { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountAgency> DiscountAgencies { get; set; }
        public DbSet<VoidService> VoidServices { get; set; }
        public DbSet<VoidServiceAgency> VoidServiceAgencies { get; set; }
        public DbSet<WalletSetting> WalletSetting { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Agent>()
                .HasOne(a => a.Agency)
                .WithMany(ag => ag.Agents)
                .HasForeignKey(a => a.AgencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Agent)
                .WithMany()
                .HasForeignKey(b => b.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Agencies
            modelBuilder.Entity<Agency>().HasData(
                new Agency { Id = "ak13jfei", AgencyName = "Global Travels", Email = "global@travel.com", Phone = "1234567890", Address = "Manama, Bahrain", IsActive = false },
                new Agency { Id = "13asd4134", AgencyName = "Sky Travels", Email = "skytravel123@gmail.com", Phone = "9078675", Address = "Pakistan, Toba Tek Singh", IsActive = false },
                new Agency { Id = "2148ekfja", AgencyName = "Falcon Tours", Email = "falcon@tours.com", Phone = "48764665", Address = "Dubai", IsActive = true },
                new Agency { Id = "kafj938ka", AgencyName = "Emirates Travel", Email = "emirates@travel.com", Phone = "90274012", Address = "Bahrain Manama", IsActive = false },
                new Agency { Id = "kjf98932", AgencyName = "Gulf Wings", Email = "gulf@wings.com", Phone = "48764665", Address = "Manama", IsActive = false }
            );

            // Seed Agents
            modelBuilder.Entity<Agent>().HasData(
                new Agent { Id = "akdjf;au332", AgentName = "Yasir Ali", UserName = "yasir294", Email = "yasir294@gmail.com", Password = "password123", IsActive = true, AgencyId = "kjf98932" },
                new Agent { Id = "qiukdaj233", AgentName = "Mubashar Ali", UserName = "mubashar", Email = "mubashar@example.com", Password = "password123", IsActive = true, AgencyId = "kafj938ka" },
                new Agent { Id = "akdjioweiu", AgentName = "Ali Khan", UserName = "alikhan", Email = "ali@example.com", Password = "password123", IsActive = true, AgencyId = "13asd4134" }
            );

            // Seed Bookings
            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = "akdjf832", GroupName = "Group A", PassengerName = "Ali Khan", PNR = "PNR12345", FlightNo = "FL001", Segment = "LHE - BAH", SeatsSold = 25, AgentId = "akdjf;au332" },
                new Booking { Id = "kja;iou29", GroupName = "Group B", PassengerName = "Fatima Zahra", PNR = "PNR67890", FlightNo = "FL002", Segment = "LHE - BAH", SeatsSold = 18, AgentId = "qiukdaj233" },
                new Booking { Id = "akldja933", GroupName = "Group C", PassengerName = "Omar Faisal", PNR = "PNR11122", FlightNo = "FL003", Segment = "LHE - BAH", SeatsSold = 30, AgentId = "akdjf;au332" }
            );
        }

    }
}