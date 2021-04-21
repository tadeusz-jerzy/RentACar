using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Core.Entities;

namespace RentACar.Infrastructure
{
    public class CarMakeConfiguration : IEntityTypeConfiguration<CarMake>
    {

        public void Configure(EntityTypeBuilder<CarMake> builder)
        {
            builder.HasIndex(m => m.Name).IsUnique();
        }
    }
}
