using _01_Framework.Infrastructure;
using _01_Query.Contract.Article;
using _01_Query.Contract.ArticleCategory;
using _01_Query.Query;
using BlogManagement.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.Configuration.Permissions;
using BlogManagement.Infrastructure.EFCore;
using BlogManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogManagement.Infrastructure.Configuration
{
    public class BlogManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IArticleCategoryApplication, ArticleCategoryApplication>();
            services.AddTransient<IArticleCategoryRepository, ArticleCategoryRepository>();

            services.AddTransient<IArticleApplication, ArticleApplication>();
            services.AddTransient<IArticleRepository, ArticleRepository>();

            services.AddTransient<IArticleQuery, ArticleQuery>();
            services.AddTransient<IArticleCategoryQuery, ArticleCategoryQuery>();

            services.AddTransient<IPermissionExposer, BlogPermissionExposer>();

            services.AddDbContext<BlogContext>(x => x.UseSqlServer(connectionString));
        }
    }
}