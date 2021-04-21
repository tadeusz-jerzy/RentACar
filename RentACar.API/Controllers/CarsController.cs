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


namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class CarsController : ControllerBase
    {
        public const int CACHE_MAX_AGE_SECONDS = 3 * Startup.CACHE_MAX_AGE_SECONDS; // 3 x
        // TODO add related links, perhaps an ApiResponse wrapper with links


        private readonly ICarService _carService;
        
        public CarsController(ICarService carService)
        {
            _carService = carService;
        }


        // GET: api/cars
        [HttpGet(Name =nameof(GetCars))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<CarForListingDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        
        // this cache setting is seen by external client
        // but not via MS-recommended testing with WebApplicationFactory
        [ResponseCache(VaryByQueryKeys = new[] { "*" }, Duration = CACHE_MAX_AGE_SECONDS, Location = ResponseCacheLocation.Any, NoStore = false)]

        public async Task<ActionResult<List<CarForListingDTO>>> GetCars([FromQuery]CarQueryFilter filters)
        {
            List<CarForListingDTO> cars = await _carService.GetCarsAsync(filters);
            
            if (cars == null || cars.Count == 0)
                return NotFound();
            
            return Ok(cars);
        }


        // GET api/cars/5
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ResponseCache(Duration = CACHE_MAX_AGE_SECONDS, Location = ResponseCacheLocation.Any, NoStore = false)]
        [HttpGet("{id}", Name = nameof(GetOneCar))]
        public async Task<ActionResult<CarForListingDTO>> GetOneCar(int id)
        {
            var car = await _carService.GetCarAsync(id);
            
            if (car == null)
                return NotFound();

            return Ok(car);
        }


        // POST api/<CarsController>
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CarForListingDTO>> Post([FromBody] CarCreateDTO createDTO)
        {
            CarForListingDTO newDto = await _carService.CreateCarFromDto(createDTO);
            return CreatedAtRoute(nameof(GetOneCar), new { id = newDto.Id }, newDto);
        }


        // DELETE api/<CarsController>/5
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var car = await _carService.GetCarAsync(id);

            if (car == null)
                return NotFound(); 
            
            await _carService.DeleteCarAsync(id);

            return Ok(true);
        }

    }
}
