using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Dtos
{
    public class VehicleInfoDto
    {
        public int Id { get; set; }

        public string TeamName { get; set; }

        public string Model { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public string Status { get; set; }

        public double DistanceReached { get; set; }

        public string VehicleType { get; set; }

        public string VehicleSubtype { get; set; }
    }
}
