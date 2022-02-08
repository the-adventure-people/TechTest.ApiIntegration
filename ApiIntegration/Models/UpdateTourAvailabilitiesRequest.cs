using ApiIntegration.ProviderModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Models
{
    public class UpdateTourAvailabilitiesRequest
    {
        public Provider Provider { get; set; }
        public Tour Tour { get; set; }
        public List<Availability> Availabilities { get; set; }
    }
}
