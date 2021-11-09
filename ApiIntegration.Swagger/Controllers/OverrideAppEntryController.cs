using ApiIntegration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration.Swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OverrideAppEntryController : ControllerBase
    {
        private readonly ILogger<OverrideAppEntryController> _logger;
        private readonly IImporter importer;
        public OverrideAppEntryController(ILogger<OverrideAppEntryController> logger, IImporter importerInstance)
        {
            this.importer = importerInstance;
            _logger = logger;
        }

        [HttpGet]
        public async Task RunApplication()
        {
            await this.importer.Execute(1);
        }
    }
}
