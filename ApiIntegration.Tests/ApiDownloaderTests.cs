using ApiIntegration.Interfaces;
using NUnit.Framework;
using System;

namespace ApiIntegration.Tests
{
    [TestFixture]
    public class ApiDownloaderTests
    {
        [Test]
        public void DownloadData_ValidUrl_ShouldSucceed()
        {
            IApiDownloader apiDownloader = new ApiDownloader("http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");

            var res = apiDownloader.Download();

            try
            {
                res.Wait();
            }
            catch (AggregateException ae)
            {
                Assert.Fail($"ApiDownloader failed with exception {ae.InnerException}");
            }

            
            Assert.IsNotNull(res.Result, "ApiDownloader failed to retrieve valid data");
            Assert.AreEqual(res.Result.StatusCode, 200);
            Assert.IsTrue(res.Result.Body.Count > 0);
        }


        [Test]
        public void DownloadData_InvalidUrl_ShouldFail()
        {
            IApiDownloader apiDownloader = new ApiDownloader("abcdefghijklmnop");

            var res = apiDownloader.Download();

            try
            {
                res.Wait();
                Assert.Fail($"ApiDownloader failed to throw any exception");
            }
            catch (AggregateException ae)
            {
                Assert.Pass($"ApiDownloader throw valid exception {ae.InnerException}");
            }
        }


        [Test]
        public void DownloadData_ValidUrl_WrongData_ShouldFail()
        {
            IApiDownloader apiDownloader = new ApiDownloader("http://google.com");

            var res = apiDownloader.Download();

            try
            {
                res.Wait();
                Assert.Fail($"ApiDownloader failed to throw any exception");
            }
            catch (AggregateException ae)
            {
                Assert.Pass($"ApiDownloader throw valid exception {ae.InnerException}");
            }
        }
    }
}
