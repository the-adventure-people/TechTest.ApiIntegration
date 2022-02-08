using ApiIntegration.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Interfaces
{
    public interface ITourPricing
    {
        decimal CalculateWebsitePrice(CalculateWebsitePriceRequest request);
    }
}
