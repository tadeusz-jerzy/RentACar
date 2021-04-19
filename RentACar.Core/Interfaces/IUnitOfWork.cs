using RentACar.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Interfaces
{

    /*
     * EF core dbContext is a unit of work already so this is more to wrap repositories
     * 
     */

    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Booking> Bookings { get;  }
        ICarRepository Cars { get;  }
        Task SaveChangesAsync();
    }
}
