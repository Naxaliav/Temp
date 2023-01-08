using FluentAssertions;
using WebApplication5.ToPort;
using Xunit;

namespace TestProject1;

public class TextFormatterExampleTests
{
    [Fact]
    public void Should_SelfLog_Exception()
    {
        var errorMessage = string.Empty;
        Serilog.Debugging.SelfLog.Enable(s => errorMessage = s);

        var formatter = new TextFormatterExample();
        formatter.Format(null, null);

        errorMessage.Should().Contain(
            "Event at  with message template  could not be formatted into JSON and will be dropped: " +
            "System.ArgumentNullException: Value cannot be null. (Parameter 'logEvent')");
    }
}