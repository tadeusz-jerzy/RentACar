using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities;
using RentACar.Core.Interfaces;
using System.Threading.Tasks;

namespace RentACar.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyAppContext _context;

        private CarRepository _cars;
        private BaseRepository<Booking> _bookings;
        
        public UnitOfWork(MyAppContext context)
        {
            _context = context;
        }

       
        public IBaseRepository<Booking> Bookings =>
            _bookings ??= new BaseRepository<Booking>(_context);

        public ICarRepository Cars =>
            _cars ??= new CarRepository(_context);

        
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
