using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Test.App
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IImporter _importer;

        public App(ILoggerFactory loggerFactory, IImporter importer)
        {
            _logger = loggerFactory.CreateLogger<App>();
            _importer = importer;
        }

        public async Task RunAsync()
        {
            await _importer.ExecuteAsync(1);
        }
    }
}
