using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities;
using RentACar.Core.Enumerations;
using RentACar.Core.Interfaces;
using RentACar.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Infrastructure
{
    // inheriting from BaseRepository allows to write more complex queries in the data/infrastructure project
    // as need arises (below examples are admittedly simple)
    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        public CarRepository(DbContext context) : base(context) { }

        public async Task<Car> GetByVinAsync(string vinCode)
            => await _entities
                .Where(c => c.Vin.Code == vinCode)
                .FirstOrDefaultAsync();
        public async Task<Car> GetByRegistrationAsync(string registration)
            => await _entities
                .Where(c => c.RegistrationNumber == registration)
                .FirstOrDefaultAsync();

        

    }
}
