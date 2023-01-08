using FluentAssertions;
using WebApplication5.ToPort;
using Xunit;

namespace TestProject1;

public class TextFormatterExampleTests
{
    [Fact]
    public void Should_SelfLog_Exception()
    {
        // Arrange
        var selfLogMessage = string.Empty;
        Serilog.Debugging.SelfLog.Enable(s => selfLogMessage = s);
        var formatter = new TextFormatterExample();

        // Act
        formatter.Format(null, null);

        // Assert
        selfLogMessage.Should().Contain(
            "Event at  with message template  could not be formatted into JSON and will be dropped: " +
            "System.ArgumentNullException: Value cannot be null. (Parameter 'logEvent')");
    }
}
