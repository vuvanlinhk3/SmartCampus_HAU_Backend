using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SmartCampus_HAU_Backend.Models.Entities;

namespace SmartCampus_HAU_Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<RoomDevice> RoomDevices => Set<RoomDevice>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<FloorPlan> FloorPlans => Set<FloorPlan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName)
                .IsUnique();

            modelBuilder.Entity<Room>()
                .HasIndex(x => x.RoomName)
                .IsUnique();

            modelBuilder.Entity<RoomDevice>()
                .ToTable(tb => tb.HasCheckConstraint("CK_RoomDevices_Quantity_NonNegative", "[quantity] >= 0"));

            modelBuilder.Entity<Booking>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Bookings_StartPeriod_1_12", "[start_period] >= 1 AND [start_period] <= 12"));

            modelBuilder.Entity<Booking>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Bookings_Periods_1_12", "[periods] >= 1 AND [periods] <= 12"));

            modelBuilder.Entity<Booking>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<RoomDevice>()
                .HasOne(x => x.Room)
                .WithMany(r => r.RoomDevices)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Unit>()
                .HasOne(x => x.Room)
                .WithMany(r => r.Units)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(x => x.Room)
                .WithMany(r => r.Bookings)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FloorPlan>()
                .HasIndex(x => x.FloorNumber)
                .IsUnique();
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
