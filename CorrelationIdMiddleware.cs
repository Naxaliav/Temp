using WebApplication5.Contracts;

namespace WebApplication5.ToPort;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ICorrelationIdProvider _correlationIdProvider;

    public CorrelationIdMiddleware(RequestDelegate next, ICorrelationIdProvider correlationIdProvider)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _correlationIdProvider = correlationIdProvider ?? throw new ArgumentNullException(nameof(correlationIdProvider));
    }

    public Task Invoke(HttpContext httpContext)
    {
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var correlationId = httpContext.Request.GetCorrelationId();
        if (correlationId is not null)
            _correlationIdProvider.SetCorrelationId(correlationId);

        httpContext.Response.Headers.TryAdd("X-CorrelationId", _correlationIdProvider.GetCorrelationId());

        return _next(httpContext);
    }
}