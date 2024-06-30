using Microsoft.EntityFrameworkCore;
using MtdKey.InboxMediator;
using MtdKey.InboxMediator.Service;
using MtdKey.InboxMediator.Worker;
using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using MtdKey.OrderMaker.Services.FileStorage;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);

string orderMakerData = builder.Configuration.GetConnectionString("OrderMakerData") ?? string.Empty;
builder.Services.AddDbContext<OrderMakerContext>(options =>
    options.UseMySql(orderMakerData, ServerVersion.AutoDetect(orderMakerData)));

//builder.Services.AddHostedService<MessageLoader>();
builder.Services.AddHostedService<FormLoader>();
builder.Services.AddInboxMediator(options => options.ConnectionString = builder.Configuration.GetConnectionString("InboxMediator") ?? string.Empty);
builder.Services.Configure<FileStorageOption>(builder.Configuration.GetSection(nameof(FileStorageOption)));
builder.Services.AddScoped<IFileStorageService, FileStorageService<OrderMakerContext>>();
builder.Services.AddScoped<IEmailMediatorReader, EmailMediatorReader<FormLoaderHandler>>();
var host = builder.Build();
host.Run();
