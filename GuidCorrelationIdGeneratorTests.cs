using FluentAssertions;
using WebApplication5.ToPort;
using Xunit;

namespace TestProject1;

public class GuidCorrelationIdGeneratorTests
{
    [Fact]
    public void GenerateCorrelationId_Should_Return()
    {
        // Arrange
        var sut = new GuidCorrelationIdGenerator();

        // Act & Assert
        sut.GenerateCorrelationId().Should().HaveLength($"{Guid.Empty}".Length).And.NotBeNullOrWhiteSpace();
    }
}