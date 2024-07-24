using DeliveryManagement.Domain.DeliveryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryManagement.Infrastructure.EfCore.Mapping
{
    public class DeliveryMapping : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("Deliveris");
            builder.Property(x => x.Address).HasMaxLength(512).IsRequired();
            builder.Property(x => x.PostalCode).HasMaxLength(10).IsRequired();
        }
    }
}
