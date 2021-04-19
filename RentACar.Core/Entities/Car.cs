using Microsoft.EntityFrameworkCore;
using RentACar.Core.DTOs;
using RentACar.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentACar.Core.Entities
{
    // contains business rules validation that can be checked within Car
    // (validations that require repository access / other entities are in Services.CarService)

    public class Car : BaseEntity
    {
        private Car() { }
        
        [Required]
        public Vin Vin { get; set; }
        
        // Registration Number could be a ValueObject with own validation too
        [Required]
        [MinLength(8)]
        public string RegistrationNumber { get; set; }

        [Required]
        public CarSpecification Specification { get ; set ; }

        [DataType(DataType.Currency)]
        [Range(9,5000, ErrorMessage = "Daily price {0} must be between {1} and {2}")]
        public decimal DailyPricePLN { get; set; } 

        [Required]
        public RentalCarStatus Status { get; set; } 

        public List<Booking> Bookings { get; set; } 
        
        
        public bool IsActive => (RentalCarStatus.Active == Status);

        
        internal static Car FromDto (CarCreateDTO dto)
        {
            // checks domain-level rules internally
            var spec = CarSpecification.FromMakeModelAndAcriss(
                    dto.Make, dto.Model, dto.AcrissCode);

            var vin = new Vin(dto.Vin);

            return new Car()
            {
                Specification = spec,
                Vin = vin,
                DailyPricePLN = dto.DailyPricePLN,
                Status = RentalCarStatus.Active
            };            
        }

    }
}
