using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Models
{
    public class CalculateWebsitePriceRequest
    {
        public decimal ProviderPrice { get; set; }
        public decimal ComissionPercentage { get; set; } 
    }
}
