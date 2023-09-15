using Cinema.Models.Domain;
using Cinema.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repository
{
    public class ApplicationDbContext : IdentityDbContext<CinemaUser>
    {
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<CinemaUser> CinemaUsers { get; set; }
        public virtual DbSet<DateTimeKey> DateTimeKeys { get; set; }
        public virtual DbSet<SeatMap> SeatMaps { get; set; } 

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Movie>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();
            
            // builder.Entity<Order>()
            //     .HasKey(z => z.Id);
            
            // builder.Entity<Order>()
            //     .Property(z => z.Id)
            //     .ValueGeneratedOnAdd();
            
            builder.Entity<Seat>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();
            
            
            builder.Entity<DateTimeKey>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();
            
            builder.Entity<SeatMap>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();
            
            // builder.Entity<CinemaUser>()
            //     .HasMany(u => u.Orders)  // One CinemaUser has many Orders
            //     .WithOne(o => o.CinemaUser) // Each Order belongs to one CinemaUser
            //     .HasForeignKey(o => o.CinemaUser.Name); // Define the foreign key property
            
            // builder.Entity<Order>()
            //     .HasOne(o => o.CinemaUser)
            //     .WithMany(u => u.Orders)
            //     .HasForeignKey(o => o.CinemaUserId);

            

        }
    }
}


