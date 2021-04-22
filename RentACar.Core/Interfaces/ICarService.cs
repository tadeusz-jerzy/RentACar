using RentACar.Core.DTOs;
using RentACar.Core.Entities;
using RentACar.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Interfaces
{
    public interface ICarService
    {
        Task<List<CarForListingDTO>> GetCarsAsync (CarQueryFilter filters);

        Task<CarForListingDTO> GetCarAsync(int id);

        Task<CarForListingDTO> CreateCarFromDto(CarCreateDTO dto);

        Task<bool> DeleteCarAsync(int id);

    }
}
