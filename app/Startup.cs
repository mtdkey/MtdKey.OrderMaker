/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MtdKey.Cipher;
using MtdKey.EmailBuilder;
using MtdKey.InboxMediator;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Areas.Identity.Data;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services;
using MtdKey.OrderMaker.Services.FileStorage;
using MtdKey.OrderMaker.Services.Tokens;

namespace MtdKey.OrderMaker
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        public IConfiguration Configuration { get; } = configuration;
        public IWebHostEnvironment CurrentEnvironment { get; } = env;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.Secure = CookieSecurePolicy.Always;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Domain = Configuration.GetConnectionString("Domain");
                options.Cookie.Name = $".OrderMaker.{Configuration.GetConnectionString("ClientName")}";
            });

            string orderMakerIdentity = Configuration.GetConnectionString("OrderMakerIdentity");
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseMySql(orderMakerIdentity, ServerVersion.AutoDetect(orderMakerIdentity)));

            string orderMakerData = Configuration.GetConnectionString("OrderMakerData");
            services.AddDbContext<OrderMakerContext>(options =>
                options.UseMySql(orderMakerData, ServerVersion.AutoDetect(orderMakerData)));
            services.AddHostedService<MigrationService>();

            services.AddDataProtection()
            .SetApplicationName($"{Configuration.GetConnectionString("ClientName")}")
            .PersistKeysToDbContext<IdentityDbContext>();

            services.AddDefaultIdentity<WebAppUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
                config.User.RequireUniqueEmail = true;

            }).AddRoles<WebAppRole>()
             .AddEntityFrameworkStores<IdentityDbContext>()
             .AddDefaultTokenProviders();

            Program.TemplateConnectionStaring = Configuration.GetConnectionString("Template");

            services.AddMemoryCache();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RoleAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RoleUser", policy => policy.RequireRole("User", "Admin"));
                options.AddPolicy("RoleGuest", policy => policy.RequireRole("Guest", "User", "Admin"));
            });

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddDataAnnotationsLocalization(options =>
                    {
                        options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                    })
                .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.AuthorizeFolder("/");
                        options.Conventions.AuthorizeAreaFolder("Workplace", "/", "RoleUser");
                        options.Conventions.AuthorizeAreaFolder("Identity", "/Users", "RoleAdmin");
                        options.Conventions.AuthorizeAreaFolder("Config", "/", "RoleAdmin");
                    });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddScoped<UserHandler>();
            services.AddTransient<ConfigHandler>();
            services.AddTransient<IEmailSenderBlank, EmailSender>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<ConfigSettings>(Configuration.GetSection("ConfigSettings"));
            services.Configure<LimitSettings>(Configuration.GetSection("LimitSettings"));
            services.AddSingleton<ITemplateStorage, FileTemplates>();
            services.AddSingleton<IAppParams, AppParams>();
            services.AddScoped<ITokenService, TokenService<IdentityDbContext>>();
            services.AddScoped<RegistrationService>();
            services.Configure<FileStorageOption>(Configuration.GetSection(nameof(FileStorageOption)));
            services.AddScoped<IFileStorageService, FileStorageService<OrderMakerContext>>();

            services.AddAesMangerService(options =>
            {
                options.SecretKey = Configuration.GetValue<string>("AesOptions:SecretKey");
                options.KeySize = Configuration.GetValue<int>("AesOptions:KeySize");
            });


            services.AddCoreServices();

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.AddScoped<DataConnector>();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddInboxMediator(options => options.ConnectionString = Configuration.GetConnectionString("InboxMediator"));
            
#if DEBUG
            services.AddRazorPages().AddRazorRuntimeCompilation();
#endif
          
        }

        public void Configure(IApplicationBuilder app)
        {

            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseUsersMiddleware();
            app.UseLocalizerMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.UseMvc();

        }
    }
}
