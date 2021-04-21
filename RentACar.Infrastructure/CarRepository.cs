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
    // as need arises (below examples are admittedly simple - small optimization to avoid returning whole entity from db)
    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        public CarRepository(DbContext context) : base(context) { }

        public async Task<bool> CarWithVinExists(string vinCode)
            => (null != await _entities
                .Where(c => c.Vin.Code == vinCode)
                .Include(c => c.Id)
                .FirstOrDefaultAsync());
        public async Task<bool> CarWithRegistrationExists(string reg)
            => (null != await _entities
                .Where(c => c.RegistrationNumber == reg)
                .Include(c => c.Id)
                .FirstOrDefaultAsync());



    }
}
