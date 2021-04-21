using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RentACar.Core.DTOs;
using RentACar.Core.Entities;
using RentACar.Core.Exceptions;
using RentACar.Core.Interfaces;
using RentACar.Core.QueryFilters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using RentACar.Core.Enumerations;
using System.Linq;

namespace RentACar.Core.Services
{
    // handles CRUD for car makes and models (within car make aggregate root)
    public class CarMakeService : ICarMakeService 
    {

        private readonly IUnitOfWork _db;
        public CarMakeService(IUnitOfWork db)
        {
            _db = db;
        }

        private async Task<CarMake> FindByNameAsync(string soughtName)
         => await _db.CarMakes.GetOneAsync(
             make => make.Name == soughtName.ToUpper());

        private async Task<CarModel> FindByNameAndMakeAsync(string soughtName, int carMakeId)
             => await _db.CarModels.GetOneAsync(
                 make => make.CarMakeId == carMakeId, 
                 make => make.Name == soughtName.ToUpper());


        public async Task<CarMakeDTO> CreateCarMakeFromDtoAsync(CarMakeDTO createDto)
        {
            CarMake preExisting = await FindByNameAsync(createDto.Name);
            if (preExisting != null)
                throw new ConflictingEntityException("Car make with the same name already exists");

            var make = new CarMake { Name = createDto.Name } ;
            _db.CarMakes.Add(make);
            await _db.SaveChangesAsync();

            return MyMapper.Map(make);                
        }

        public async Task<CarModelDTO> CreateCarModelFromDtoAsync(CarModelDTO createDto )
        {
            CarMake make = await _db.CarMakes.FindByIdAsync(createDto.CarMakeId);
            if (make == null)
                throw new EntityNotFoundException("car make id must point to an existing car make");

            CarModel preExisting = await FindByNameAndMakeAsync(createDto.Name, createDto.CarMakeId);
            if (preExisting != null)
                throw new ConflictingEntityException("Car model with the same name already exists within this make");

            CarModel entity = new CarModel
            {
                Name = createDto.Name,
                CarMake = make,
                CarMakeId = createDto.CarMakeId
            };

            _db.CarModels.Add(entity);
            _db.SaveChangesAsync();

            return MyMapper.Map(entity);
        }

        public async Task<CarModelDTO> GetCarModelAsync(int id)
            => MyMapper.Map(
                await _db.CarModels.FindByIdAsync(id));
       

        public async Task<List<CarModelDTO>> GetCarModelsForMakeAsync(int id)
        {
            var entities = await _db.CarModels.GetManyAsync(cm => cm.CarMakeId == id);
            var dtos = entities.Select(m => MyMapper.Map(m)).ToList();
            return dtos;
        }

        public async Task<CarMakeDTO> GetMakeAsync(int id)
        {
            CarMake entity = await _db.CarMakes.FindByIdAsync(id);
            return MyMapper.Map(entity);
        }

        public async Task<List<CarMakeDTO>> GetMakesAsync()
        {
            List<CarMake> entities = await _db.CarMakes.GetAllAsync();
            var dtos = entities.Select(e => MyMapper.Map(e)).ToList();
            return dtos;
        }
    }
}
