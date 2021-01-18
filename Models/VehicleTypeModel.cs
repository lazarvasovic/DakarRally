using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Models
{
    public class VehicleTypeModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string TypeName { get; set; }

        [Required]
        public double RepairLength { get; set; }
    }
}
