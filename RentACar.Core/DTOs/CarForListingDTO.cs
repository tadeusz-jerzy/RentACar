using RentACar.Core.Entities;
namespace RentACar.Core.DTOs
{
    public class CarForListingDTO
    {
        public int Id { get; set; }
        
        public string Make { get; set; }
        public string Model { get; set; }
        public string AcrissCode { get; set; }
        public decimal DailyPricePLN { get; set; }

        public static implicit operator CarForListingDTO(Car v)
        {
            return MyMapper.Map(v);
        }
    }
}
