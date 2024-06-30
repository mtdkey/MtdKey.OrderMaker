using Microsoft.Extensions.DependencyInjection;
using MtdKey.InboxMediator.Data;
using MtdKey.InboxMediator.Service;

namespace MtdKey.InboxMediator.Worker
{
    public class MessageLoader(ILogger<MessageLoader> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<MessageLoader> _logger = logger;
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using IServiceScope scope = serviceScopeFactory.CreateScope();

                IEmailMediatorWorker mediatorWorker =
                    scope.ServiceProvider.GetRequiredService<IEmailMediatorWorker>();

                IEmailMediatorService emailMediator =
                    scope.ServiceProvider.GetRequiredService<IEmailMediatorService>();

                var mediators = await emailMediator.GetAllMediatorsAsync();

                await RunServiceAsync(mediators, mediatorWorker);


                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("Message Loader running at: {time}", DateTimeOffset.Now);


                await Task.Delay(10000, stoppingToken);
            }
        }


        private async Task RunServiceAsync(EmailMediator[] mediators, IEmailMediatorWorker mediatorWorker)
        {
            try
            {
                foreach (var mediator in mediators)
                    await mediatorWorker.LoadHeadersAsync(mediator);
            }
            catch (Exception ex)
            {
                _logger.LogError("{message}", ex.Message);
            }
        }
    }
}
