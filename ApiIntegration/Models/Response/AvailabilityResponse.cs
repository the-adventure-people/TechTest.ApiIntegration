using Newtonsoft.Json;
using System;

namespace ApiIntegration.Models.Response
{
    public class AvailabilityResponse
    {
        public int TourId { get; set; }
        [JsonProperty("productCode")]
        public string ProductCode { get; set; }
        [JsonProperty("departureDate")]
        public DateTime DepartureDate { get; set; }
        [JsonProperty("nights")]
        public int Nights { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("spaces")]
        public int Spaces { get; set; }

        public void SetTourId(int tourId) 
            => TourId = tourId;

        public void AdjustPrice(decimal commission)
            => Price *= (commission + 1);
    }
}
