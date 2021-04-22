using RentACar.Core.DTOs;
using RentACar.Core.Entities;
using RentACar.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Interfaces
{
    public interface ICarMakeService
    {
        Task<List<CarMakeDTO>> GetMakesAsync ();

        Task<CarMakeDTO> GetMakeAsync(int id);

        Task<CarMakeDTO> CreateCarMakeFromDtoAsync(CarMakeDTO createDto);
        Task<CarModelDTO> CreateCarModelFromDtoAsync(CarModelDTO createDto);

        Task<List<CarModelDTO>> GetCarModelsForMakeAsync(int id);

        Task<CarModelDTO> GetCarModelAsync(int id);

    }
}
