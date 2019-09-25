using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Core.Extensions
{
    public static class DecimalExtensions
    {

        public static decimal AdjustPrice(this decimal price, decimal commission) => price - (price * 0.05m) + (price * commission);
    }
}
