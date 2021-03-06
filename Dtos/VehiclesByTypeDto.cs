﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Dtos
{
    public class VehiclesByTypeDto
    {
        [Required]
        public string VehicleType { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
