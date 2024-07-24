using DeliveryManagement.Domain.DeliveryAgg;
using DeliveryManagement.Infrastructure.EfCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Infrastructure.EfCore
{
    public class DeliveryContext : DbContext
    {
        public DbSet<Delivery> Deliveries { get; set; }

        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
