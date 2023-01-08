using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;

namespace WebApplication5.ToPort;

public class TextFormatterExample : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        try
        {
            if (logEvent is null) throw new ArgumentNullException(nameof(logEvent));
            if (output is null) throw new ArgumentNullException(nameof(output));
        }
        catch (Exception e)
        {
            LogNonFormattableEvent(logEvent, e);
        }
    }

    private static void LogNonFormattableEvent(LogEvent? logEvent, Exception e)
    {
        SelfLog.WriteLine(
            "Event at {0} with message template {1} could not be formatted into JSON and will be dropped: {2}",
            logEvent?.Timestamp.ToString("o"),
            logEvent?.MessageTemplate.Text,
            e);
    }
}