
using Microsoft.EntityFrameworkCore;
using MtdKey.EmailBuilder;
using MtdKey.InboxMediator;
using MtdKey.InboxMediator.Service;
using MtdKey.InboxMediator.Worker;
using MtdKey.OrderMaker.AppConfig;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Core.Approval;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services.EmailService;
using MtdKey.OrderMaker.Services.FileStorage;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);

string orderMakerData = builder.Configuration.GetConnectionString("OrderMakerData") ?? string.Empty;
builder.Services.AddDbContext<OrderMakerContext>(options =>
    options.UseMySql(orderMakerData, ServerVersion.AutoDetect(orderMakerData)));


string orderMakerIdentity = builder.Configuration.GetConnectionString("OrderMakerIdentity") ?? string.Empty;
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseMySql(orderMakerIdentity, ServerVersion.AutoDetect(orderMakerIdentity)));


builder.Services.AddInboxMediator(options => options.ConnectionString = builder.Configuration.GetConnectionString("InboxMediator") ?? string.Empty);
builder.Services.Configure<FileStorageOption>(builder.Configuration.GetSection(nameof(FileStorageOption)));
builder.Services.AddScoped<IFileStorageService, FileStorageService<OrderMakerContext>>();
builder.Services.AddScoped<IEmailService, EmailService<OrderMakerContext>>();
builder.Services.AddScoped<IEmailMediatorReader, EmailMediatorReader<FormLoaderHandler>>();
builder.Services.AddScoped<ITemplateStorage, FileTemplates>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IApprovalService, ApprovalService>();



builder.Services.AddHostedService<MessageLoader>();
builder.Services.AddHostedService<FormLoader>();


var host = builder.Build();
host.Run();
