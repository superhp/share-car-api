using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareCar.Db.Entities;

namespace ShareCar.Db
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RideRequest> Requests { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<DriverNote> DriverNotes { get; set; }
        public DbSet<DriverSeenNote> DriverSeenNotes { get; set; }
        public DbSet<RideRequestNote> RideRequestNotes { get; set; }
        public DbSet<UnauthorizedUser> UnauthorizedUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RideRequest>()
    .HasKey(x => x.RideRequestId);
            modelBuilder.Entity<Passenger>()
                .HasKey(x => new { x.Email, x.RideId });
            modelBuilder.Entity<Route>()
                .HasIndex(u => u.Geometry)
                .IsUnique();

            modelBuilder.Entity<Ride>()
                .Property(x => x.NumberOfSeats)
                .HasDefaultValue(4);

            modelBuilder.Entity<RideRequestNote>()
             .Property(x => x.Seen)
             .HasDefaultValue(true);

            modelBuilder.Entity<DriverSeenNote>()
            .Property(x => x.Seen)
            .HasDefaultValue(true);

            modelBuilder.Entity<Ride>()
                .Property(x => x.isActive)
                .HasDefaultValue(true);

        }
    }
}