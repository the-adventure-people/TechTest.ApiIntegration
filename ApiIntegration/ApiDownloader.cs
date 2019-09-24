using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        private readonly string Url;


        public ApiDownloader(string Url)
        {
            this.Url = Url;
        }

        Task<ApiAvailabilityResponse> IApiDownloader.Download()
        {
            return Task.Run(GetApiAvailabilityResponse);
        }

        private ApiAvailabilityResponse GetApiAvailabilityResponse()
        {
            ApiAvailabilityResponse res = new ApiAvailabilityResponse();

            using (var httpClient = new HttpClient())
            {
                var json = httpClient.GetStringAsync(Url);
                JObject response = JObject.Parse(json.Result);
                res.StatusCode = int.Parse(response["statusCode"].ToString());

                JArray availabilities = (JArray)response["body"];
                res.Body = new List<Availability>();

                foreach (var avail in availabilities)
                {
                    res.Body.Add(new Availability
                    {
                        ProductCode = avail["productCode"].ToString(),
                        DepartureDate = avail["departureDate"].ToString(),
                        Nights = int.Parse(avail["nights"].ToString()),
                        Price = int.Parse(avail["price"].ToString()),
                        Spaces = int.Parse(avail["spaces"].ToString())
                    });
                }
            }

            return res;
        }
    }
}
