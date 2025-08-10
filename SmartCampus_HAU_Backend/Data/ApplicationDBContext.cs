using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique constraints
            modelBuilder.Entity<Room>()
                .HasIndex(x => x.RoomName)
                .IsUnique();

            // Check constraints
            modelBuilder.Entity<RoomDevice>()
                .ToTable(tb => tb.HasCheckConstraint("CK_RoomDevices_Quantity_NonNegative", "[quantity] >= 0"));

            modelBuilder.Entity<Booking>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Bookings_StartPeriod_1_12", "[start_period] >= 1 AND [start_period] <= 12"));

            modelBuilder.Entity<Booking>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Bookings_Periods_1_12", "[periods] >= 1 AND [periods] <= 12"));

            // Default values
            modelBuilder.Entity<RoomDevice>()
                .Property(x => x.Status)
                .HasDefaultValue(false);

            modelBuilder.Entity<Unit>()
                .Property(x => x.Status)
                .HasDefaultValue(false);

            modelBuilder.Entity<Booking>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Delete behaviors đã được xử lý bởi [ForeignKey] và [InverseProperty]
            // Nhưng nếu muốn CASCADE rõ ràng:
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
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Connection string cho design-time (có thể giống hoặc khác runtime)
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-59E3KF3\\TEST;Initial Catalog=CampusManager;Integrated Security=True;Trust Server Certificate=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
