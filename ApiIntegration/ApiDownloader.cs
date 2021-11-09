using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        public Task<ApiAvailabilityResponse> Download()
        {
            throw new NotImplementedException();
        }
    }
}
