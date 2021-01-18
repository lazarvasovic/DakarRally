using DakarRally.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Data
{
    public class DakarDbContext : DbContext
    {
        public DakarDbContext(DbContextOptions<DakarDbContext> opt) : base(opt)
        {

        }

        public DbSet<VehicleModel> Vehicles { get; set; }
        public DbSet<MalfunctionStatisticModel> MalfunctionStatistics { get; set; }
        public DbSet<VehicleTypeModel> VehicleTypes { get; set; }
        public DbSet<VehicleSubtypeModel> VehicleSubtypes { get; set; }
        public DbSet<RaceModel> Races { get; set; }
    }
}
