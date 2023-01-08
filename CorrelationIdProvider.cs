using WebApplication5.Contracts;

namespace WebApplication5.ToPort;

public sealed class CorrelationIdProvider : ICorrelationIdProvider
{
    private static readonly AsyncLocal<string?> CorrelationIdAsyncLocal = new();

    private readonly ICorrelationIdGenerator _correlationIdGenerator;

    public CorrelationIdProvider(ICorrelationIdGenerator correlationIdGenerator)
    {
        _correlationIdGenerator = correlationIdGenerator ?? throw new ArgumentNullException(nameof(correlationIdGenerator));
    }

    public void SetCorrelationId(string correlationId)
    {
        CorrelationIdAsyncLocal.Value = correlationId;
    }

    public string GetCorrelationId()
    {
        if (string.IsNullOrWhiteSpace(CorrelationIdAsyncLocal.Value))
            CorrelationIdAsyncLocal.Value = _correlationIdGenerator.GenerateCorrelationId();

        return CorrelationIdAsyncLocal.Value;
    }
}