using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MtdKey.InboxMediator.Data;
using MtdKey.InboxMediator.Service;
using Pomelo.EntityFrameworkCore.MySql.Internal;


namespace MtdKey.InboxMediator
{
    public static class ServiceExtensions
    {
        public class InboxMediatorOption
        {
            public string ConnectionString { get; set; } = string.Empty;
        }

        public static IServiceCollection AddInboxMediator(this IServiceCollection services, Action<InboxMediatorOption> options)
        {
            InboxMediatorOption mediatorOption = new();
            options.Invoke(mediatorOption);
            string connectionString = mediatorOption.ConnectionString; 

            services.AddDbContext<InboxMediatorContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOptions => {
                    mysqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                }));
            services.AddHostedService<MigrationService>();

            services.TryAddScoped<IEmailMediatorService, EmailMediatorService>();
            services.TryAddScoped<IEmailMediatorWorker, EmailMediatorWorker>();
            //services.TryAddScoped<IEmailMediatorReader, EmailMediatorReader>();

            return services;
        }

    }
}
