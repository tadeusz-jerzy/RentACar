using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentACar.Core.DTOs
{
    public class CarMakeDTO
    {
        public int Id { get; set; }
        
        [MinLength(1)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
