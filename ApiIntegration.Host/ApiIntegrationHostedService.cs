namespace ApiIntegration.Host
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using ApiIntegration.Import;
    using ApiIntegration.Providers.AwesomeCyclingHolidays.Models;

    using Microsoft.Extensions.Hosting;

    public class ApiIntegrationHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IImporter<AvailabilityResponse> _importer;

        public ApiIntegrationHostedService(
            IHostApplicationLifetime lifetime,
            IImporter<AvailabilityResponse> importer)
        {
            _lifetime = lifetime;
            _importer = importer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _importer.ExecuteAsync(1, cancellationToken);

            _lifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
