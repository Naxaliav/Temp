public static class HttpRequestExtensions
{
    public static string? GetCorrelationId(this HttpRequest httpRequest)
    {
        if (httpRequest == null)
            throw new ArgumentNullException(nameof(httpRequest));

        if (httpRequest.Headers.TryGetValue("X-Correlation-ID", out var correlationId) &&
            !string.IsNullOrWhiteSpace(correlationId))
        {
            return correlationId;
        }

        return null;
    }

    public static string? GetForwardedUrl(this HttpRequest httpRequest)
    {
        if (httpRequest == null)
            throw new ArgumentNullException(nameof(httpRequest));

        if (httpRequest.Headers.TryGetValue("X-Forwarded-Proto", out var forwardedProto)
            && httpRequest.Headers.TryGetValue("X-Forwarded-Host", out var forwardedHost))
        {
            return $"{forwardedProto}://{forwardedHost}{httpRequest.Path}{httpRequest.QueryString}";
        }

        return null;
    }

    public static string GetOriginalUrl(this HttpRequest httpRequest)
    {
        if (httpRequest == null)
            throw new ArgumentNullException(nameof(httpRequest));

        if (httpRequest.Headers.TryGetValue("X-Original-Proto", out var originalProto)
            && httpRequest.Headers.TryGetValue("X-Original-Host", out var originalHost))
        {
            return $"{originalProto}://{originalHost}{httpRequest.Path}{httpRequest.QueryString}";
        }

        return $"{httpRequest.Scheme}://{httpRequest.Host.Host}{httpRequest.Path}{httpRequest.QueryString}";
    }
}