using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        public async Task<ApiAvailabilityResponse> Download()
        {
            string rawdata = string.Empty;
            string url = @"http://tap.techtest.s3-website.eu-west-2.amazonaws.com/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                rawdata = await reader.ReadToEndAsync();
            }
            ApiAvailabilityResponse apiresponse = (ApiAvailabilityResponse)JsonConvert.DeserializeObject(rawdata, typeof(ApiAvailabilityResponse));

            return apiresponse;
        }
    }
}
