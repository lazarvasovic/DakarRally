using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DakarRally.Models
{
    public class VehicleModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string TeamName { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public DateTime ManufacturingDate { get; set; }

        [Required]
        public VehicleStatus VehicleStatus { get; set; }

        [Required]
        public VehicleSubtypeModel VehicleSubtype { get; set; }

        [Required]
        public RaceModel Race { get; set; }

        public double DistanceReached { get; set; }

        public double RepairTimeLeft { get; set; }

        public double FinishTime { get; set; }
    }
}