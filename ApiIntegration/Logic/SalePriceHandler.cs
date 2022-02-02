using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Logic {
    public class SalePriceHandler : IPriceHandler {
        public const decimal SALE_DISCOUNT = 0.05M;
        public SalePriceHandler(ILogger<SalePriceHandler> logger) {
            Logger = logger;
        }

        public ILogger<SalePriceHandler> Logger { get; }

        public decimal GetPrice(decimal providerPrice, decimal commission) {
            var calculatedDiscount = providerPrice * SALE_DISCOUNT;
            var calculatedComission = providerPrice * commission;
            return providerPrice + calculatedComission - calculatedDiscount;
        }
    }
}
