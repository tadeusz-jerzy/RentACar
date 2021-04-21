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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
