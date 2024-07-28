using _01_Framework.Application;
using AccountManagement.Configuration.Permissions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace ServiceHost
{
    public class SecurityPageFilter : IPageFilter
    {
        private readonly IAuthHelper _authHelper;

        public SecurityPageFilter(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {

        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            try
            {
            var handlerPermission = (NeedPermissionAttribute)context.HandlerMethod.MethodInfo.GetCustomAttribute(typeof(NeedPermissionAttribute));

            if (handlerPermission == null)
                return;

            var accountPermissions = _authHelper.GetPermissions();

            if (accountPermissions.All(x => x != handlerPermission.Permission))
                context.HttpContext.Response.Redirect("/Account");
            }
            catch
            {
                return;
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {

        }
    }
}
