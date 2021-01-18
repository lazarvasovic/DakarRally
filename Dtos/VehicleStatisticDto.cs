using DakarRally.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Dtos
{
    public class VehicleStatisticDto
    {
        public int Id { get; set; }

        public string TeamName { get; set; }

        public string Model { get; set; }

        public double DistanceReached { get; set; }

        public string Status { get; set; }

        public List<MalfunctionStatisticDto> Malfunctions { get; set; }

        public double FinishTime { get; set; }
    }
}
