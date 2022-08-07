namespace ApiIntegration.Host
{
    using ApiIntegration.Finance;
    using ApiIntegration.Import;
    using ApiIntegration.Providers;
    using ApiIntegration.Providers.AwesomeCyclingHolidays;
    using ApiIntegration.Providers.AwesomeCyclingHolidays.Models;
    using ApiIntegration.Repositories;

    using FluentValidation;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ApiIntegrationModule
    {
        public static void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            // REPOSITORIES
            services.AddSingleton<ITourRepository, TourRepository>();
            services.AddSingleton<IProviderRepository, ProviderRepository>();

            // GENERIC SERVICES
            services.AddScoped<ITourPricingService, TourPricingService>();

            // PROVIDER SPECIFIC SERVICES
            // TODO - Sort generic registration of all provider imports.
            services.Configure<Settings>(context.Configuration.GetSection("AwesomeCyclingHolidays"));
            services.AddScoped<IImporter<AvailabilityResponse>, Importer<AvailabilityResponse>>();
            services.AddHttpClient<IAvailabilityApi<AvailabilityResponse>, AvailabilityApi>();
            services.AddScoped<IValidator<AvailabilityResponse>, AvailabilityResponseValidator>();
            services.AddScoped<IValidator<Availability>, AvailabilityValidator>();
            services.AddScoped<IProviderResponseAdapter<AvailabilityResponse>, AvailabilityResponseAdapter>();
        }
    }
}
