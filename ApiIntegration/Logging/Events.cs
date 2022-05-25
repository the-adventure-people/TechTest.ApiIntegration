using Microsoft.Extensions.Logging;

namespace ApiIntegration.Logging
{
    public static class Events
    {
        public static EventId ImporterService = new EventId(1, "ImporterService");
        public static EventId ApiDownloaderService = new EventId(2, "ApiDownloaderService");
        public static EventId TourService = new EventId(4, "TourService");
        public static EventId ProviderService = new EventId(5, "ProviderService");
    }
}
