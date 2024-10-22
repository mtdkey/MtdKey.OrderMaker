using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace MtdKey.InboxMediator.Data
{
    public class MigrationService : IHostedService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger logger;

        public MigrationService(IServiceScopeFactory serviceScopeFactory,
            ILogger<MigrationService> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<InboxMediatorContext>();


            IEnumerable<string> pm = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);

            try
            {
                if(pm.Any())
                    await dbContext.Database.MigrateAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                logger.LogError("Error: {message}", message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


    }
}
