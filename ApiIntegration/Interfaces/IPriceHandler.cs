using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Interfaces {
    public interface IPriceHandler {
        decimal GetPrice(decimal providerPrice, decimal comission);
    }
}
