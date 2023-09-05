using _01_Framework.Infrastructure;
using CommandManagement.Application;
using CommandManagement.Domain.CommentAgg;
using CommentManagement.Infrastructure.Configuration.Permissions;
using CommentManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application.Contracts.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentManagement.Infrastructure.Configuration
{
    public class CommentManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string ConnectionString)
        {
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICommentApplication, CommentApplication>();

            services.AddTransient<IPermissionExposer, CommentPermissionExposer>();

            services.AddDbContext<CommentContext>(x => x.UseSqlServer(ConnectionString));
        }
    }
}
