using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Models
{
    public class MalfunctionStatisticModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public MalfunctionType MalfunctionType { get; set; }

        [Required]
        public double Time { get; set; }

        [Required]
        public VehicleModel Vehicle { get; set; }
    }
}
