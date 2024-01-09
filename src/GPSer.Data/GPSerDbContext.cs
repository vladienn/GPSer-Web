using GPSer.Model;
using Microsoft.EntityFrameworkCore;

namespace GPSer.Data;

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
    }
}