using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Logic {
    public class PriceHandler : IPriceHandler {
        public PriceHandler(ILogger<PriceHandler> logger) {
            Logger = logger;
        }

        public ILogger<PriceHandler> Logger { get; }

        public decimal GetPrice(decimal providerPrice, decimal commission, decimal discount) {
            if (discount >= 1){
                Logger.LogWarning("Discount is greater than 100%");
                throw new Exception("Discount is greater than 100%");
            }
            var calculatedDiscount = providerPrice * discount;
            var calculatedComission = providerPrice * commission;
            return providerPrice + calculatedComission - calculatedDiscount;
        }
    }
}
