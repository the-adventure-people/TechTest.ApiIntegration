namespace ApiIntegration.Providers.AwesomeCyclingHolidays.Models
{
    using System.Collections.Generic;

    public class AvailabilityResponse
    {
        public int StatusCode { get; init; }
        public List<Availability> Body { get; init; }
    }
    public class Availability
    {
        public string ProductCode { get; init; } = string.Empty;
        public DateTime DepartureDate { get; init; }
        public int Nights { get; init; }
        public decimal Price { get; init; }
        public int Spaces { get; init; }
    }
}
