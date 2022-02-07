using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Tests.TestData
{
    internal class AvailabilityApi
    {
        public static string IncorrectBody = "test";
        // Copied from http://tap.techtest.s3-website.eu-west-2.amazonaws.com/
        public static string ExampleBody = @"{
    ""statusCode"": 200,
    ""body"": [
        {
            ""productCode"": ""EUR123"",
            ""departureDate"": ""2020-06-20"",
            ""nights"": 5,
            ""price"": 500.0,
            ""spaces"": 8
        },
        {
            ""productCode"": ""EUR123"",
            ""departureDate"": ""2020-06-27"",
            ""nights"": 5,
            ""price"": 450.0,
            ""spaces"": 4
        },
        {
    ""productCode"": ""EUR123"",
            ""departureDate"": ""2020-07-04"",
            ""nights"": 5,
            ""price"": 500.0,
            ""spaces"": 6
        },
        {
    ""productCode"": ""EUR456"",
            ""departureDate"": ""2020-03-10"",
            ""nights"": 10,
            ""price"": 800.0,
            ""spaces"": 4
        },
        {
    ""productCode"": ""EUR456"",
            ""departureDate"": ""2020-03-20"",
            ""nights"": 10,
            ""price"": 800.0,
            ""spaces"": 5
        },
        {
    ""productCode"": ""EUR789"",
            ""departureDate"": ""2020-09-20"",
            ""nights"": 4,
            ""price"": 250.0,
            ""spaces"": 9
        }
    ]
}";
    }
}
