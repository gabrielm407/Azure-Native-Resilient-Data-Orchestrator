using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        
        // Register Service Bus Client
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnection"));
        });
    })
    .Build();

host.Run();