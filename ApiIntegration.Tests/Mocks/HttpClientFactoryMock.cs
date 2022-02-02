namespace ApiIntegration.Tests.Mocks
{
    using ApiIntegration.ProviderModels;
    using Moq;
    using Moq.Protected;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpClientFactoryMock : IHttpClientFactory
    {
        private readonly string _baseAddress;

        public HttpClientFactoryMock(string baseAddress = "http://fake.endpoint")
        {
            _baseAddress = baseAddress;
        }

        public HttpClient CreateClient(string name)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
               {
                   var requestUrl = request.RequestUri.AbsoluteUri;

                   // fake 200
                   if (requestUrl == "http://fake.endpoint/")
                   {
                       return new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new ObjectContent<ApiAvailabilityResponse>(TestData.TestAvailabilityData(), new JsonMediaTypeFormatter())
                       };
                   };

                   // fake 400 
                   if (requestUrl == "http://fake.endpoint400/")
                   {
                       return new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.BadRequest
                       };
                   };

                   // fake 404
                   return new HttpResponseMessage
                   {
                       StatusCode = HttpStatusCode.NotFound
                   };
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(_baseAddress)
            };

            return httpClient;
        }
    }
}
