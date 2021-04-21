using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentACar.Core.DTOs
{
    public class CarModelDTO
    {
        public int Id { get; set; }
        public int CarMakeId { get; set; }


        [MinLength(1)]
        [MaxLength(100)]
        string LogoFilename { get; set; } // optional


        [MinLength(1)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }


    }
}
