using ApiIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    internal class TourPricing : ITourPricing
    {
        public TourPricing()
        {

        }

        private async Task<decimal> AdjustPrice(decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
