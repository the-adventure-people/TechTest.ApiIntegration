namespace ApiIntegration.Tests.Extensions
{
    using System;
    using System.IO;
    using ApiIntegration.Extensions;
    using FluentAssertions;
    using Xunit;

    public class DateTimeParsingTests
    {
        [Fact(DisplayName = "Parse valid datetime")]
        public void Parse_Valid_DateTime()
        {
            "2020-06-20".ToDateTime().Should().Be(new DateTime(2020, 06, 20));
        }

        [Fact(DisplayName = "Parse should throw exception")]
        public void Parse_Should_Throw_Exception()
        {
            Action act = () => "".ToDateTime();

            act.Should().Throw<InvalidDataException>();
        }
    }
}