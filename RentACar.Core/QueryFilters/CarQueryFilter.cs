using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.QueryFilters
{
    public class CarQueryFilter
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string AcrissCode { get; set; }
        public decimal MinDailyPricePLN{ get; set; }
        public decimal MaxDailyPricePLN { get; set; }
        public DateTime RequestedRentalStart { get; set; }
        public DateTime RequestedRentalEnd { get; set; }
    }
}
