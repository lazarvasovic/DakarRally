using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Models
{
    public class VehicleSubtypeModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string SubtypeName { get; set; }

        [Required]
        public double MaxSpeed { get; set; }

        [Required]
        public double LigthMalfunProbab { get; set; }

        [Required]
        public double HeavyMalfunProbab { get; set; }

        [Required]
        public VehicleTypeModel VehicleType { get; set; }
    }
}
