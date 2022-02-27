using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class TourService
    {
        private IProviderRepository providerRepository;
        private TourRepository tourRepository;

        public TourService(IProviderRepository providerRepository)
        {
            this.providerRepository = providerRepository;
            tourRepository = new TourRepository();
        }
        private async Task<decimal> AdjustPrice(decimal price, int providerId)
        {
            var provider = await providerRepository.Get(providerId);
            var providerCommission = price * provider.Commission;
            var discount = price * 0.05m;
            return price + providerCommission - discount;
        }

        public void AdjustAllPrices(List<TourAvailability> tourAvailabilities, int providerId)
        {
            tourAvailabilities.ForEach(async x => x.SellingPrice = await AdjustPrice(x.SellingPrice, providerId));
        }

        public async Task<List<TourAvailability>> ConvertToTourAvailability(ApiAvailabilityResponse apiAvailabilityResponse)
        {
            var tours = new List<TourAvailability>();

            foreach (var availability in apiAvailabilityResponse.Body)
            {
                var tour = await tourRepository.Get(0, availability.ProductCode);
                if (tour != null)
                {
                    tours.Add(new TourAvailability()
                    {
                        AvailabilityCount = availability.Spaces,
                        SellingPrice = availability.Price,
                        StartDate = DateTime.Parse(availability.DepartureDate),
                        TourDuration = availability.Nights,
                        TourId = tour.TourId
                    });
                }                
            }

            return tours;
        }

        public async Task UpdateAvailabilities(List<TourAvailability> tourAvailabilities)
        {            
            foreach(var availability in tourAvailabilities)
            {
                var tour = await tourRepository.Get(availability.TourId, String.Empty);
                tour.Availabilities.Add(availability);
                await tourRepository.Update(tour);
            }
        }
    }
}
