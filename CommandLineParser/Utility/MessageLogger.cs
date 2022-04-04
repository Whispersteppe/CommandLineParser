using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Utility
{
    /// <summary>
    /// Message
    /// </summary>
    public class Message
    {
        public LogLevel Level { get; set; }
        public EventId ID { get; set; }
        public Exception? Exception { get; set; }
        public string? MessageData { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(ID.ToString());
            sb.Append("/t");
            sb.Append(Level.ToString());
            sb.Append("/t");
            sb.Append(MessageData);

            return sb.ToString();
        }
    }

    /// <summary>
    /// internal message logger
    /// </summary>
    public class MessageLogger : ILogger
    {

        public List<Message> Messages { get; set; } = new List<Message>();

        public bool HasErrors
        {
            get
            {
                return Messages.Any(x => x.Level >= LogLevel.Error);
            }
        }
        public bool HasWarnings
        {
            get
            {
                return Messages.Any(x => x.Level >= LogLevel.Warning);
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                Message newMsg = new()
                {
                    Level = logLevel,
                    Exception = exception,
                    ID = eventId,
                    MessageData = formatter(state, exception)
                };

                Messages.Add(newMsg);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
