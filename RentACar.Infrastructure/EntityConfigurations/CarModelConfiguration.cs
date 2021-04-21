using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Core.Entities;

namespace RentACar.Infrastructure
{
    public class CarModelConfiguration : IEntityTypeConfiguration<CarModel>
    {
        public void Configure(EntityTypeBuilder<CarModel> builder)
        {
            builder.HasOne(m => m.CarMake)
                .WithMany(m => m.CarModels);

            builder.HasIndex(m => m.CarMakeId);
           
        }
    }
}
