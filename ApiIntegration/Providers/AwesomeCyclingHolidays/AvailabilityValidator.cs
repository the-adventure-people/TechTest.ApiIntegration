namespace ApiIntegration.Providers.AwesomeCyclingHolidays
{
    using System;

    using ApiIntegration.Providers.AwesomeCyclingHolidays.Models;

    using FluentValidation;

    public class AvailabilityValidator : AbstractValidator<Availability?>
    {
        public AvailabilityValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r!.ProductCode).NotNull().NotEmpty();
            RuleFor(r => r!.DepartureDate.ToUniversalTime()).GreaterThanOrEqualTo(DateTime.MinValue);
            RuleFor(r => r!.Nights).GreaterThanOrEqualTo(0);
            RuleFor(r => r!.Price).GreaterThanOrEqualTo(0);
            RuleFor(r => r!.Spaces).GreaterThanOrEqualTo(0);
        }
    }
}
