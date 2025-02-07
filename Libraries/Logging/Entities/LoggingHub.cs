using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace ThePalace.Logging.Entities
{
    public class LoggingHub : ILogger
    {
        public LoggingHub()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                //.WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();
        }
        public LoggingHub(ILogger logger) : this()
        {
            Logger = logger;
        }
        public LoggingHub(IConfiguration configuration) : this()
        {
            _configuration = configuration;
        }
        public LoggingHub(ILogger logger, IConfiguration configuration)
        {
            _configuration = configuration;

            Logger = logger;
        }

        private readonly IConfiguration _configuration;
        public ILogger Logger { get; internal set; }

        public void Write(LogEvent logEvent)
        {
            Logger.Write(logEvent);
        }
    }
}