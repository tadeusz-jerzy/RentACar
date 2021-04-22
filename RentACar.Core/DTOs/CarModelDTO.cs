using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentACar.Core.DTOs
{
    public class CarModelDTO
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "CarMakeId must be at least 1 (and point to a valid car make entity)")]
        public int CarMakeId { get; set; }


        [MinLength(1)]
        [MaxLength(100)]
        string LogoFilename { get; set; } 


        [MinLength(1)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }


    }
}
