using _01_Framework.Infrastructure;
using _01_Query.Contract.Inventory;
using _01_Query.Query;
using InventoryManagement.Application;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using InventoryManagement.Infrastructure.Configuration.Permissions;
using InventoryManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Infrastructure.Configuration
{
    public class InventoryManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IInventoryApplication, InventoryApplication>();
            services.AddTransient<IInventoryRepository, InventoryRepository>();

            services.AddTransient<IPermissionExposer, InventoryPermissionExposer>();

            services.AddTransient<IInventoryQuery, InventoryQuery>();

            services.AddDbContext<InventoryContext>(x => x.UseSqlServer(connectionString));
        }
    }
}