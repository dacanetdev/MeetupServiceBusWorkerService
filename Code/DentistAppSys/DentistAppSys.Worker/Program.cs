using DentistAppSys.Domain.Services;
using DentistAppSys.Worker;
using Microsoft.Extensions.Azure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddAzureClients(builder => builder.AddServiceBusClient(
                hostContext.Configuration.GetConnectionString("ServiceBus")
            ));
        services.AddSingleton<IMailService, MailService>();
    })
    .Build();

await host.RunAsync();
