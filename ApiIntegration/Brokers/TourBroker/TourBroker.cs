using ApiIntegration.Interfaces;
using ApiIntegration.Models.ResponseModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core.Brokers.TourBroker
{
    public class TourBroker : ITourBroker
    {
        //Static - -we only want one instance to avoid any port problems also probably read the url from an appsettings .
        //https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient _client = new HttpClient();
        private readonly ILogger logger;

        public TourBroker(ILogger logger)
        {
            this.logger = logger;
            //Generally id inject an http client here from the DI container , going to create a static client for now .
        }

      
       
        public async Task<ApiAvailabilityResponse> GetTourDataAsync()
        {
            try
            {
                //there should be a route in here and a base address set on the http client - I guess this is ok for now as its only one endpoint
                var result = await _client.GetStringAsync("http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");
                return JsonConvert.DeserializeObject<ApiAvailabilityResponse>(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while communicating with provider API.");
                throw;
            }
            

        }
    }
}
