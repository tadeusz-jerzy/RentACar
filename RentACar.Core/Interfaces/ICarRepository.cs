using RentACar.Core.Entities;
using RentACar.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentACar.Core.Interfaces
{
    /// <summary>
    /// contains a specialized query for finding available cars
    /// </summary>
    public interface ICarRepository: IBaseRepository<Car>
    {
        // TZ: business logic like GetAvailableCars does not really belong in the repo layer
        // and Core project is also somewhat coupled with ORM 
        // 
        // but sometimes writing specialized queries
        // may need to happen closer to the actual ORM
        // (while writing code in Core will not require deep EF knowledge from the devs)
        // 
        // so sometimes we could implement a more speicalized repo than the BaseRepository
        Task<bool> CarWithVinExists(string vinCode);
        Task<bool> CarWithRegistrationExists(string registration);

        
    }
}
