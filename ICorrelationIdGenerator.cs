namespace WebApplication5.Contracts;

public interface ICorrelationIdGenerator
{
    string GenerateCorrelationId();
}