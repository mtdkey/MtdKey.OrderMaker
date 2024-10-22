using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Data
{
    public class IdentityMigrationService : IHostedService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger logger;

        public IdentityMigrationService(IServiceScopeFactory serviceScopeFactory,
            ILogger<MigrationService> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<OrderMakerContext>();

            var idContext = scope.ServiceProvider
                .GetRequiredService<IdentityDbContext>();

            bool idMigration = false;

            try
            {

                IEnumerable<string> idPending = await idContext.Database.GetPendingMigrationsAsync(cancellationToken);
                idMigration = idPending.Any();

                if (idMigration)
                {

                    await idContext.Database.MigrateAsync(cancellationToken);

                    using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WebAppUser>>();
                    bool usersExists = await userManager.Users.AnyAsync(cancellationToken: cancellationToken);
                    if (usersExists == false)
                    {
                        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<WebAppRole>>();
                        await InitIdentity(scope.ServiceProvider, roleManager, userManager);
                    }
                }


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


        private static async Task InitIdentity(IServiceProvider serviceProvider,
            RoleManager<WebAppRole> roleManager,
            UserManager<WebAppUser> userManager)
        {

            var roleAdmin = new WebAppRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Title = "Administrator",
                Seq = 30
            };

            var roleUser = new WebAppRole
            {
                Name = "User",
                NormalizedName = "USER",
                Title = "User",
                Seq = 20
            };

            var roleGuest = new WebAppRole
            {
                Name = "Guest",
                NormalizedName = "GUEST",
                Title = "Guest",
                Seq = 10
            };

            await roleManager.CreateAsync(roleAdmin);
            await roleManager.CreateAsync(roleUser);
            await roleManager.CreateAsync(roleGuest);

            var config = serviceProvider.GetRequiredService<IOptions<ConfigSettings>>();

            var defaultAdmin = new WebAppUser
            {
                Email = config.Value.EmailSupport,
                EmailConfirmed = true,
                Title = "Administrator",
                UserName = config.Value.DefaultUSR,
            };

            await userManager.CreateAsync(defaultAdmin, config.Value.DefaultPWD);
            await userManager.AddToRoleAsync(defaultAdmin, "Admin");

        }

    }
}
