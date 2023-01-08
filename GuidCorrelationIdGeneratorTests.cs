using FluentAssertions;
using WebApplication5.ToPort;
using Xunit;

namespace TestProject1;

public class GuidCorrelationIdGeneratorTests
{
    [Fact]
    public void GenerateCorrelationId_Should_Return_Unique_Guid()
    {
        // Arrange
        var sut = new GuidCorrelationIdGenerator();
        var results = new List<string>();
        var iterationsCount = 10;

        // Act
        for (var i = 0; i < iterationsCount; i++)
           results.Add(sut.GenerateCorrelationId());

        // Assert
        results.Distinct().Count().Should().Be(iterationsCount);
        results.Should().AllSatisfy(result => Guid.Parse(result));
    }
}
