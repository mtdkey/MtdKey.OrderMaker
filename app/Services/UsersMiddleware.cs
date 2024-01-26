using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MtdKey.OrderMaker.Areas.Identity.Data;
using System;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Services
{
    public class UsersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public UsersMiddleware(RequestDelegate next,IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var value = Guid.Empty;
            using var scope = serviceScopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WebAppUser>>();

            if (context.User.Identity.Name != null)
            {
                var user = await userManager.GetUserAsync(context.User);
                value = user?.DatabaseId ?? Guid.Empty;
            }
            context.Items["databaseId"] = value.ToString();
            await _next(context);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseUsersMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UsersMiddleware>();
        }
    }
}
