﻿using ApiIntegration.ProviderModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.ProviderModels
{
    internal class ImportAvailabilitiesRequest
    {
        public List<Availability> Availabilities { get; set; }
        public int ProviderId { get; set; }
    }
}