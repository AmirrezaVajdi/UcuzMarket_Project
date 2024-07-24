using DeliveryManagement.Application;
using DeliveryManagement.Application.Contract.Delivery;
using DeliveryManagement.Domain.DeliveryAgg;
using DeliveryManagement.Infrastructure.EfCore;
using DeliveryManagement.Infrastructure.EfCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryManagement.Infrastructure.Configuration
{
    public class DeliveryManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connctionString)
        {
            services.AddTransient<IDeliveryRepository, DeliveryRepository>();
            services.AddTransient<IDeliveryApplication, DeliveryApplication>();

            services.AddDbContext<DeliveryContext>(x => x.UseSqlServer(connctionString));
        }
    }
}