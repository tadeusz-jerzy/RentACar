using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentACar.Infrastructure;
using RentACar.Core.Entities;
using System;
using System.Reflection;

namespace RentACar.Infrastructure
{
    public partial class MyAppContext : DbContext
    {
        public MyAppContext() { }

        public MyAppContext(DbContextOptions<MyAppContext> options)
            : base(options) {

        }
        /*
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Availability> Availabilities { get; set; }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
