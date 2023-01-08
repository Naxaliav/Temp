using WebApplication5.Contracts;

namespace WebApplication5.ToPort;

public sealed class GuidCorrelationIdGenerator : ICorrelationIdGenerator
{
    public string GenerateCorrelationId() => $"{Guid.NewGuid()}";
}