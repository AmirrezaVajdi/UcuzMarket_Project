using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Infrasturecure.EFCore.Mapping
{
    internal class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IssueTrackingNo).HasMaxLength(8).IsRequired(false);
            builder.OwnsMany(x => x.Items, builder =>
            {
                builder.ToTable("OrderItems");
                builder.HasKey(x => x.Id);
                builder.WithOwner(x => x.Order).HasForeignKey(x => x.OrderId);
            });
        }
    }
}
