using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace TestProject1
{
    public class HttpRequestExtensionsTests
    {
        private readonly DefaultHttpContext _httpContext = new();

        [Theory]
        [InlineData("c4f6dfa2-8f61-11ed-a1eb-0242ac120002", "c4f6dfa2-8f61-11ed-a1eb-0242ac120002")]
        [InlineData(null, null)]
        [InlineData(" ", null)]
        [InlineData("", null)]
        public void GetCorrelationId_Should_Return(string? inputCorrelationId, string? expectedCorrelationId)
        {
            // Arrange
            _httpContext.Request.Headers.Add("X-Correlation-Id", inputCorrelationId);

            // Act & Assert
            _httpContext.Request.GetCorrelationId().Should().Be(expectedCorrelationId);
        }

        [Fact]
        public void GetOriginalUrl_Should_Return()
        {
            // Arrange
            SetHttpContextRequest(scheme: "https", host: "google.com", path: "/api/v1/redemption-offer", query: "?parameter=123");

            // Act & Assert
            _httpContext.Request.GetOriginalUrl().Should().Be("https://google.com/api/v1/redemption-offer?parameter=123");
        }

        [Fact]
        public void GetOriginalUrl_WithOriginalHeaderSet_Should_Return()
        {
            // Arrange
            SetHttpContextRequest(scheme: "http", host: "localhost", path: "/api/v1/redemption-offer", query: "?parameter=123");
            _httpContext.Request.Headers.Add("X-Original-Proto", "https");
            _httpContext.Request.Headers.Add("X-Original-Host", "google.com");

            // Act & Assert
            _httpContext.Request.GetOriginalUrl().Should().Be("https://google.com/api/v1/redemption-offer?parameter=123");
        }


        [Fact]
        public void GetForwardedUrl_Should_Return()
        {
            // Act & Assert
            _httpContext.Request.GetForwardedUrl().Should().BeNull();
        }

        [Fact]
        public void GetForwardedUrl_WithForwardedHeadersSet_Should_Return()
        {
            // Arrange
            SetHttpContextRequest(scheme: "http", host: "localhost", path: "/api/v1/redemption-offer", query: "?parameter=123");

            _httpContext.Request.Headers.Add("X-Forwarded-Proto", "https");
            _httpContext.Request.Headers.Add("X-Forwarded-Host", "google.com");

            // Act & Assert
            _httpContext.Request.GetForwardedUrl().Should().Be("https://google.com/api/v1/redemption-offer?parameter=123");
        }

        [Fact]
        public void Should_ThrowArgumentNullException()
        {
            // Act & Assert
            FluentActions.Invoking(((HttpRequest)null).GetCorrelationId).Should().Throw<ArgumentNullException>();
            FluentActions.Invoking(((HttpRequest)null).GetForwardedUrl).Should().Throw<ArgumentNullException>();
            FluentActions.Invoking(((HttpRequest)null).GetOriginalUrl).Should().Throw<ArgumentNullException>();
        }

        private void SetHttpContextRequest(string scheme, string host, string path, string query)
        {
            _httpContext.Request.Scheme = scheme;
            _httpContext.Request.Host = new HostString(host);
            _httpContext.Request.Path = path;
            _httpContext.Request.QueryString = new QueryString(query);
        }
    }
}