﻿using GPSer.API.Models;
using GPSer.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}