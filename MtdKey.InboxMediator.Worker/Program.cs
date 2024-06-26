using MtdKey.InboxMediator;
using MtdKey.InboxMediator.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddInboxMediator(options => options.ConnectionString = builder.Configuration.GetConnectionString("InboxMediator") ?? string.Empty);
var host = builder.Build();
host.Run();
