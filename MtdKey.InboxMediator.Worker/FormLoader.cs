
using Microsoft.EntityFrameworkCore;
using MtdKey.InboxMediator.Service;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services.EmailService;
using System.Reflection.Metadata;

namespace MtdKey.InboxMediator.Worker
{
    public class FormLoader(ILogger<MessageLoader> logger, 
        IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<MessageLoader> _logger = logger;
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory;
       

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            OrderMakerContext context =
                  scope.ServiceProvider.GetRequiredService<OrderMakerContext>();

            IEmailMediatorReader reader =
                scope.ServiceProvider.GetRequiredService<IEmailMediatorReader>();
            IEmailMediatorService emailMediator =
                scope.ServiceProvider.GetRequiredService<IEmailMediatorService>();
            IEmailService emailService =
                scope.ServiceProvider.GetRequiredService<IEmailService>();


            while (!stoppingToken.IsCancellationRequested)
            {

                var mediators = await emailMediator.GetActiveMediatorsAsync();
                foreach (var mediator in mediators)
                {
                    var filters = await context.TargetFilters
                        .Include(x => x.TargetForm)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .Where(x=>x.EmailAsKey == mediator.EmailAsKey)
                        .ToListAsync(cancellationToken: stoppingToken);

                    foreach(var filter in filters)
                        await reader.HandleNewEmailsAsync(new ReaderModel { 
                            Id = Guid.Parse(filter.TargetForm.FormId), 
                            EmailMediator = mediator, 
                            Filter = filter.Filter 
                        });

                }

                var taskIds = await emailService.GetActiveTasksAsync();
                
                foreach (var taskId in taskIds)
                    await emailService.SendEmailAsync(taskId);
                

                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("Form Loader running at: {time}", DateTimeOffset.Now);


                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
