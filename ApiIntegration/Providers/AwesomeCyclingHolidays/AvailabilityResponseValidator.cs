namespace ApiIntegration.Providers.AwesomeCyclingHolidays
{
    using ApiIntegration.Providers.AwesomeCyclingHolidays.Models;

    using FluentValidation;

    public class AvailabilityResponseValidator : AbstractValidator<AvailabilityResponse?>
    {
        public AvailabilityResponseValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r!.StatusCode).Equal(200);
            RuleFor(r => r!.Body).NotNull();
        }
    }
}
