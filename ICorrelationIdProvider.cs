namespace WebApplication5.Contracts;

public interface ICorrelationIdProvider
{
    public void SetCorrelationId(string correlationId);
    public string GetCorrelationId();
}