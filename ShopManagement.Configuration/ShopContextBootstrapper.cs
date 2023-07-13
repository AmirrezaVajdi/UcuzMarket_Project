using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Infrasturecure.EFCore;
using ShopManagement.Infrasturecure.EFCore.Repository;

namespace ShopManagement.Configuration
{
    public class ShopContextBootstrapper
    {
        public static void Configure(IServiceCollection services , string connectionString)
        {
            services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();

            services.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
