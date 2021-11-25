﻿namespace ApiIntegration.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string ApiEndpoint { get; set; }
        public decimal Commission { get; set; }
        public decimal Discount { get; set; }
    }
}
