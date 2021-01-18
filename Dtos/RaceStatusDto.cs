using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Dtos
{
    public class RaceStatusDto
    {
        public double TimeElapsed { get; set; }

        public string RaceStatus { get; set; }

        public List<VehiclesByStatusDto> VehiclesByStatus { get; set; }

        public List<VehiclesByTypeDto> VehiclesByType { get; set; }
    }
}
