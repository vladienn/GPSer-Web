using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GPSer.Data;

public class GPSerDbContext : IdentityDbContext<IdentityUser>
{
    public GPSerDbContext(DbContextOptions<GPSerDbContext> options)
        : base(options)
    {
    }

    //public DbSet<Device>? Devices { get; set; }
    //public DbSet<LocationData>? LocationDatas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}