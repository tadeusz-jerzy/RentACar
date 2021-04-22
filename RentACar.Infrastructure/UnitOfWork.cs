using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities;
using RentACar.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace RentACar.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyAppContext _context;

        private CarRepository _cars;
        private BaseRepository<Booking> _bookings;
        private BaseRepository<CarMake> _carMakes;
        private BaseRepository<CarModel> _carModels;

        public UnitOfWork(MyAppContext context)
        {
            _context = context;
        }

       
        public IBaseRepository<Booking> Bookings =>
            _bookings ??= new BaseRepository<Booking>(_context);

        public IBaseRepository<CarMake> CarMakes =>
            _carMakes ??= new BaseRepository<CarMake>(_context);

        public IBaseRepository<CarModel> CarModels =>
            _carModels ??= new BaseRepository<CarModel>(_context);

        public ICarRepository Cars =>
            _cars ??= new CarRepository(_context);

        
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
