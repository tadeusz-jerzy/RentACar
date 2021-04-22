using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentACar.Core.Entities
{
    public class CarModel : BaseEntity
    {
        public int CarMakeId { get; set; }
        public CarMake CarMake { get; set; }

        [MinLength(1)]
        [MaxLength(100)]
        [Required] 
        public string Name { get; set; }
    }
}
