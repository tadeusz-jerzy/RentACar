using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.Core.Entities;
using RentACar.Core.Exceptions;
using RentACar.Core.Interfaces;
using RentACar.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        // TODO add related links, perhaps an ApiResponse wrapper with links
        // controller 

        private readonly ICarService _carService;
        
        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        // GET: api/cars
        [HttpGet(Name =nameof(GetCars))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<CarForListingDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<List<CarForListingDTO>>> GetCars([FromQuery]CarQueryFilter filters)
        {
            List<CarForListingDTO> cars = await _carService.GetCarsAsync(filters);
            
            if (cars == null || cars.Count == 0)
                return NoContent();
            
            return Ok(cars);
        }

        // GET api/cars/5
        [HttpGet("{id}", Name = nameof(GetOneCar))]
        public async Task<ActionResult<CarForListingDTO>> GetOneCar(int id)
        {
            var car = await _carService.GetCarAsync(id);
            
            if (car == null)
                return NotFound();

            return Ok(car);
        }

        // POST api/<CarsController>
        [HttpPost]
        public async Task<ActionResult<CarForListingDTO>> Post([FromBody] CarCreateDTO createDTO)
        {
            CarForListingDTO newDto = await _carService.CreateCarFromDto(createDTO);
            return CreatedAtRoute(nameof(GetOneCar), newDto);
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _carService.DeleteCarAsync(id);
            return Ok(true);
        }

    }
}
