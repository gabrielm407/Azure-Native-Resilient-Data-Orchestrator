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

        // 🛡️ SAFETY CHECK: Only register Service Bus if the config exists
        var serviceBusConnection = Environment.GetEnvironmentVariable("ServiceBusConnection");

        if (!string.IsNullOrEmpty(serviceBusConnection))
        {
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddServiceBusClient(serviceBusConnection);
            });
        }
    })
    .Build();

host.Run();