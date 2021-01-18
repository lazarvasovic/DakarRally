using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Dtos
{
    public class VehicleUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string TeamName { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public DateTime ManufacturingDate { get; set; }

        [Required]
        public string VehicleType { get; set; }

        public string VehicleSubtype { get; set; }
    }
}
