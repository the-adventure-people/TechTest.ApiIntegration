using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class TourPricing : ITourPricing
    {
        #region Static values
        private readonly string DiscountConfigKey = "PricingDiscount";
        #endregion

        private readonly decimal PricingDiscountPercentage;

        public TourPricing(IConfiguration configuration)
        {
            PricingDiscountPercentage = Convert.ToInt32(configuration[DiscountConfigKey]);
        }

        public decimal CalculateWebsitePrice(CalculateWebsitePriceRequest req)
        {
            var comission = req.ComissionPercentage * req.ProviderPrice;
            var discount = (req.ProviderPrice * PricingDiscountPercentage / 100);
            var price = req.ProviderPrice + comission - discount;

            return price;
        }

    }
}
