using Microsoft.EntityFrameworkCore;
using RentACar.Core.DTOs;
using RentACar.Core.Services;
using RentACar.Infrastructure;
using RentACar.Tests.Shared;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace RentACar.IntegrationTests
{
    public class InMemoryDbTest : TestWithOutput, IDisposable
    {

        // https://xunit.net/docs/shared-context
        protected MyAppContext context;
        protected UnitOfWork db;

        public InMemoryDbTest(ITestOutputHelper output) : base(output)
        {
            UseFreshDb();
        }

        protected void UseFreshDb()
        {
            TestOutput.WriteLine("reset db");
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            builder.UseInMemoryDatabase("Testing");
            context = new MyAppContext(builder.Options);
            context.Database.EnsureCreated();
            db = new UnitOfWork(context);
        }

        protected MyAppContext GetNewContext()
        {
            var builder = new DbContextOptionsBuilder<MyAppContext>();
            builder.UseInMemoryDatabase("Testing");
            context = new MyAppContext(builder.Options);
            return context;
        }

        protected virtual void Dispose(bool disposing)
        {
            context.Database.EnsureDeleted();
            db.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
