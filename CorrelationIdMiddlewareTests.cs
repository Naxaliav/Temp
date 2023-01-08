using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using WebApplication5.Contracts;
using WebApplication5.ToPort;
using Xunit;

namespace TestProject1;

public class CorrelationIdMiddlewareTests
{
    private readonly Mock<ICorrelationIdProvider> _correlationIdProviderMock = new();
    private readonly DefaultHttpContext _httpContext = new();
    private CorrelationIdMiddleware Sut => new(_ => Task.CompletedTask, _correlationIdProviderMock.Object);

    [Fact]
    public void Invoke_ShouldSetResponseHeaders_ByIncomingCorrelationId()
    {
        // Arrange
        var correlationId = $"{Guid.NewGuid()};";
        _httpContext.Request.Headers.Add("X-Correlation-Id", correlationId);
        WhenCorrelationIdProviderReturns(correlationId);

        // Act
        Sut.Invoke(_httpContext);

        // Assert
        _correlationIdProviderMock.Verify(o => o.SetCorrelationId(correlationId), Times.Once);
        _httpContext.Response.Headers.Should().Contain("X-CorrelationId", correlationId);
    }

    [Fact]
    public void Invoke_ShouldSetResponseHeaders_WhenCorrelationIdIsNotProvided()
    {
        // Arrange
        var correlationId = $"{Guid.NewGuid()};";
        WhenCorrelationIdProviderReturns(correlationId);

        // Act
        Sut.Invoke(_httpContext);

        // Assert
        _httpContext.Response.Headers.Should().Contain("X-CorrelationId", correlationId);
    }

    [Fact]
    public async Task Should_ThrowArgumentNullException()
    {
        // Act & Assert
        await FluentActions.Invoking(() => Sut.Invoke(null)).Should().ThrowAsync<ArgumentNullException>();
        FluentActions.Invoking(() => new CorrelationIdMiddleware(null, null)).Should().Throw<ArgumentNullException>();
        FluentActions.Invoking(() => new CorrelationIdMiddleware(_ => Task.CompletedTask, null)).Should().Throw<ArgumentNullException>();
        FluentActions.Invoking(() => new CorrelationIdMiddleware(null, _correlationIdProviderMock.Object)).Should().Throw<ArgumentNullException>();
    }

    private void WhenCorrelationIdProviderReturns(string correlationId)
    {
        _correlationIdProviderMock.Setup(o => o.GetCorrelationId()).Returns(correlationId);
    }
}