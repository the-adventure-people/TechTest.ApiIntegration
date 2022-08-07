// See https://aka.ms/new-console-template for more information
using ApiIntegration.Host;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureLogging(lb =>
    {
        lb.AddConsole();
    })
    .ConfigureServices((context, services) =>
    {
        ApiIntegrationModule.ConfigureServices(services, context);

        services.AddHostedService<ApiIntegrationHostedService>();
    })
    .Build()
    .Run();

