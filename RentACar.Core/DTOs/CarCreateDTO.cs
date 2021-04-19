using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentACar.Core.DTOs
{
    // structure validation without business rules validation
    public class CarCreateDTO
    {
        [Required]
        [MinLength(17)]
        [MaxLength(17)]
        public string Vin { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        public string RegistrationNumber { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Make { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Model { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string AcrissCode { get; set; }
        
        [Required]
        public decimal DailyPricePLN { get; set; }
    }
}


