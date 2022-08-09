using GPSer.API.Models;
using GPSer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GPSer.Data
{
    public class GPSerDbContext : DbContext
    {
        public GPSerDbContext(DbContextOptions<GPSerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Device>? Devices { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<LocationData>? LocationDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .Property(e => e.Role)
                .HasConversion(new EnumToStringConverter<Roles>());
        }
    }
}