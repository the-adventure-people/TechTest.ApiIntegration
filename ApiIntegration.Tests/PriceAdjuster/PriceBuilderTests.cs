namespace ApiIntegration.Tests.PriceAdjuster
{
    using FluentAssertions;
    using Models;
    using Xunit;

    public class PriceBuilderTests
    {
        [Fact(DisplayName = "Build price")]
        public void Build_Price()
        {
            new PriceBuilder().Build(new Provider {Commission = 0.15m}, 100)
                .Should().Be(110);
        }
    }
}