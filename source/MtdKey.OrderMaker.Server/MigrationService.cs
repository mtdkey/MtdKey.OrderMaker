using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker
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

                await MigrationHandlerAsync(dbContext);

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

        public static async Task MigrationHandlerAsync(OrderMakerContext dbContext)
        {
            IEnumerable<string> pm = await dbContext.Database.GetPendingMigrationsAsync();
            if (pm.Any())
            {
                await dbContext.Database.MigrateAsync();
                await InitDatabaseAsync(dbContext);
            }
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

        private static async Task InitDatabaseAsync(OrderMakerContext context)
        {
            if(await context.MtdCategoryForm.AnyAsync() is not true)
            {
                var mtdGroupForm = new MtdCategoryForm
                {
                    Id = "17101180-9250-4498-BE4E-4A941AD6713C",
                    Name = "Default",
                    Description = "Default Group",
                    Parent = "17101180-9250-4498-BE4E-4A941AD6713C"
                };

                await context.MtdCategoryForm.AddAsync(mtdGroupForm);
                await context.SaveChangesAsync();
            }

            if(await context.MtdSysType.AnyAsync() is not true)
            {
                sbyte active = 1;
                List<MtdSysType> mtdSysTypes = new() {
                    new MtdSysType{ Id = 1, Name="Text", Description="Text", Active=active },
                    new MtdSysType{ Id = 2, Name="Integer", Description="Integer", Active=active},
                    new MtdSysType{ Id = 3, Name="Decimal",Description="Decimal", Active=active},
                    new MtdSysType{ Id = 4, Name = "Memo",Description="Memo",Active=active},
                    new MtdSysType{ Id = 5, Name="Date",Description="Date",Active=active},
                    new MtdSysType{ Id = 6, Name="DateTime",Description="DateTime",Active=active},
                    new MtdSysType{ Id = 7, Name="File",Description="File",Active=active},
                    new MtdSysType{ Id = 8, Name="Image",Description="Image",Active=active},
                    new MtdSysType{ Id = 11, Name="List",Description="List",Active=active},
                    new MtdSysType{ Id = 12, Name="Checkbox",Description="Checkbox",Active=active},
                    new MtdSysType{ Id = 13, Name="Link",Description="Link",Active=active},
                    new MtdSysType{ Id = 14, Name="FileStorage",Description="FileStorage",Active=active},
                    new MtdSysType{ Id = 15, Name="HTMLEditor",Description="HTMLEditor",Active=active},
                };

                await context.MtdSysType.AddRangeAsync(mtdSysTypes);
                await context.SaveChangesAsync();
            }

            if (await context.MtdSysTerm.AnyAsync() is not true)
            {
                var mtdSysTerms = new List<MtdSysTerm>
                {
                    new MtdSysTerm {Id=1,Name="equal", Sign="=" },
                    new MtdSysTerm {Id=2,Name="less", Sign="<" },
                    new MtdSysTerm {Id=3,Name="more", Sign=">" },
                    new MtdSysTerm {Id=4,Name="contains", Sign="~" },
                    new MtdSysTerm {Id=5,Name="no equal", Sign="<>" },
                };

                await context.MtdSysTerm.AddRangeAsync(mtdSysTerms);
                await context.SaveChangesAsync();
            }


            if(await context.MtdSysStyle.AnyAsync() is not true)
            {
                var mtdSysStyles = new List<MtdSysStyle>
                {
                    new MtdSysStyle{Id=4,Name="Lines", Description="Lines", Active=(sbyte)1},
                    new MtdSysStyle{Id=5,Name="Columns", Description="Columns", Active=(sbyte)1}
                };

                await context.MtdSysStyle.AddRangeAsync(mtdSysStyles);

                await context.SaveChangesAsync();
            }


        }

    }
}
