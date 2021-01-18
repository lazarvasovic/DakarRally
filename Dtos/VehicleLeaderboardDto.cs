using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Dtos
{
    public class VehicleLeaderboardDto
    {
        public int Id { get; set; }

        public int CurrentPlace { get; set; }

        public double DistanceReached { get; set; }

        public string Status { get; set; }

        public double RepairTimeLeft { get; set; }

        public string TeamName { get; set; }

        public string Model { get; set; }

        public string VehicleType { get; set; }

        public string VehicleSubtype { get; set; }
    }
}
