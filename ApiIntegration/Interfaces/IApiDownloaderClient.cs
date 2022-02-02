using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces {

    public interface IApiDownloaderClient {
        Task<HttpResponseMessage> Download();
    }
}
