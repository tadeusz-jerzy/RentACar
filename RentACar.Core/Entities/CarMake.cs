using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentACar.Core.Entities
{
    public class CarMake : BaseEntity
    {
        [MinLength(1)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MinLength(1)]
        [MaxLength(100)]
        public string LogoFilename { get; set;} // optional

        public List<CarModel> CarModels { get; set; }

    }
}
