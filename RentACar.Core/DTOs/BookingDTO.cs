using RentACar.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RentACar.Core.DTOs
{
    public class BookingDTO
    {
        // structure validation without business rules validation

        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string UserId { get; set; }
        
        [Required]
        public int CarId { get; set; }
        
        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
    }
}
