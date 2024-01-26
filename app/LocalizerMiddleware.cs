using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker
{
    public class LocalizerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public LocalizerMiddleware(RequestDelegate next,IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var value = Guid.Empty;
            using var scope = serviceScopeFactory.CreateScope();

            var language = context.GetBrouserLanguage();
            CultureInfo.CurrentCulture = new CultureInfo(language, false);
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            await _next(context);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLocalizerMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LocalizerMiddleware>();
        }

        public static string GetBrouserLanguage(this HttpContext context) 
        {
            var language = "en-US";
            var languages = context.Request.GetTypedHeaders()
                       .AcceptLanguage
                       ?.OrderByDescending(x => x.Quality ?? 1)
                       .Select(x => x.Value.ToString())
                       .ToArray() ?? Array.Empty<string>();

            if (languages.Length > 0)
            {
                language = languages[0];
                if (language.ToLower() == "ru" || language.ToLower()[..2] == "ru")
                    language = "ru-RU";
                if (language.ToLower() == "en" || language.ToLower()[..2] == "en")
                    language = "en-US";

                if (language != "ru-RU" && language != "en-US")
                    language = "en-US";

            }
            
            return language;
        }
    }
}
