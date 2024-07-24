using _01_Framework.Infrastructure;
using _01_Query;
using _01_Query.Contract.Product;
using _01_Query.Contract.ProductCategory;
using _01_Query.Contract.Slide;
using _01_Query.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.Services;
using ShopManagement.Domain.SlideAgg;
using ShopManagement.Infrastructure.AccountAcl;
using ShopManagement.Infrastructure.Configuration.Permissions;
using ShopManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore.Repository;
using ShopManagement.Infrastructure.InventoryAcl;
using ShopManagement.Infrasturecure.EFCore.Repository;

namespace ShopManagement.Infrastructure.Configuration
{
    public class ShopContextBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();

            services.AddTransient<IProductApplication, ProductApplication>();
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddTransient<IProductPictureApplication, ProductPictureApplication>();
            services.AddTransient<IProductPictureRepository, ProductPictureRepository>();

            services.AddTransient<ISlideApplication, SlideApplication>();
            services.AddTransient<ISlideRepository, SlideRepository>();

            services.AddTransient<IOrderApplication, OrderApplication>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddSingleton<ICartService, CartService>();

            services.AddTransient<ISlideQuery, SlideQuery>();
            services.AddTransient<IProductCategoryQuery, ProductCategoryQuery>();
            services.AddTransient<IProductQuery, ProductQuery>();

            services.AddTransient<IPermissionExposer, ShopPermissionExposer>();

            services.AddTransient<ICartCalculatorService, CartCalculatorService>();

            services.AddTransient<IShopInventoryAcl, ShopInventoryAcl>();
            services.AddTransient<IShopAccountAcl, ShopAccountAcl>();

            services.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
