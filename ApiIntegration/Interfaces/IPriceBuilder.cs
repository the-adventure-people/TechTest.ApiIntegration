namespace ApiIntegration.Interfaces
{
    using Models;

    public interface IPriceBuilder
    {
        decimal Build(Provider provider, decimal price);
    }
}