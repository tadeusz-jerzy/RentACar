using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Core.Interfaces;
using RentACar.Core.Services;
using System;

namespace RentACar.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyDbContexts(this IServiceCollection services,
            IConfiguration configuration)
        {
            Console.WriteLine("AddContexts");
            services.AddDbContext<MyAppContext>(o => o.UseInMemoryDatabase("RentACar_Main2"));
            return services;
        }

        public static IServiceCollection AddMyServices (this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddScoped (typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
            
        }

    }
}
