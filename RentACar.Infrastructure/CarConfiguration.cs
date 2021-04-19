using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Core.Entities;

namespace RentACar.Infrastructure
{
    public class CarConfiguration: IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.OwnsOne(c => c.Specification);
            builder.OwnsOne(c => c.Vin);
            
            // builder.HasIndex(c => c.Vin.Code).IsUnique(); doesn't seem to be supported...
            builder.HasIndex(c => c.RegistrationNumber).IsUnique();
        }
    }
}
