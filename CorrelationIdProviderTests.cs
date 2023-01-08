using FluentAssertions;
using Moq;
using WebApplication5.Contracts;
using WebApplication5.ToPort;
using Xunit;

namespace TestProject1;

public class CorrelationIdProviderTests
{
    private readonly Mock<ICorrelationIdGenerator> _correlationIdGeneratorMock = new();

    [Theory]
    [InlineData("521695c6-0dea-4584-bb45-3fe06651c727")]
    [InlineData("13b18640-945a-43d5-a83a-1b49206de0c1")]
    [InlineData("Some")]
    public void GetCorrelationId_ShouldReturnSetCorrelationId(string correlationId)
    {
        // Arrange
        var sut = new CorrelationIdProvider(new GuidCorrelationIdGenerator());
        sut.SetCorrelationId(correlationId);

        // Act & Assert
        sut.GetCorrelationId().Should().Be(correlationId);
    }

    [Fact]
    public void GetCorrelationId_ShouldReturnGeneratedCorrelationId()
    {
        // Arrange
        var sut = new CorrelationIdProvider(new GuidCorrelationIdGenerator());

        // Act & Assert
        sut.GetCorrelationId().Should().Match(guid => Guid.Parse(guid) != Guid.Empty);
    }

    [Theory]
    [InlineData("521695c6-0dea-4584-bb45-3fe06651c727")]
    [InlineData("13b18640-945a-43d5-a83a-1b49206de0c1")]
    [InlineData("Some")]
    public void GetCorrelationId_ShouldReturnSetCorrelationId_WhenCustomGeneratorIsUsed(string correlationId)
    {
        // Arrange
        WhenCorrelationIdGeneratorReturns("GeneratedValue");
        var sut = new CorrelationIdProvider(_correlationIdGeneratorMock.Object);
        sut.SetCorrelationId(correlationId);

        // Act & Assert
        sut.GetCorrelationId().Should().Be(correlationId);
    }


    [Fact]
    public void GetCorrelationId_ShouldReturnGeneratedCorrelationId_WhenCustomGeneratorIsUsed()
    {
        // Arrange
        WhenCorrelationIdGeneratorReturns("GeneratedValue");
        var sut = new CorrelationIdProvider(_correlationIdGeneratorMock.Object);

        // Act & Assert
        sut.GetCorrelationId().Should().Be("GeneratedValue");
    }

    [Fact]
    public void Should_ThrowArgumentNullException()
    {
        // Act & Assert
        FluentActions.Invoking(() => new CorrelationIdProvider(null)).Should().Throw<ArgumentNullException>();
    }

    private void WhenCorrelationIdGeneratorReturns(string returnedValue)
    {
        _correlationIdGeneratorMock.Setup(o => o.GenerateCorrelationId()).Returns(returnedValue);
    }
}
