﻿using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Lib.Logging.Entities;

public class LoggerHub : Singleton<LoggerHub>, ILogger
{
    private readonly IConfiguration _configuration;

    public LoggerHub()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile(@"Config\AppSettings.json")
            .Build();

        Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
            //.WriteTo.File(new CompactJsonFormatter(), "logs/logs")
            .CreateLogger();
    }

    public LoggerHub(ILogger logger) : this()
    {
        Logger = logger;
    }

    public LoggerHub(IConfiguration configuration) : this()
    {
        _configuration = configuration;
    }

    public LoggerHub(ILogger logger, IConfiguration configuration)
    {
        _configuration = configuration;

        Logger = logger;
    }

    public ILogger Logger { get; internal set; }

    public void Write(LogEvent logEvent)
    {
        Logger.Write(logEvent);
    }

    public void Write(LogEventLevel level, string message)
    {
        Logger.Write(level, message);
    }

    public void Console(string format, params object[]? args)
    {
        System.Console.WriteLine(format, args);
    }

    public void Verbose(string message)
    {
        Logger.Write(LogEventLevel.Verbose, message);
    }

    public void Debug(string message)
    {
        Logger.Write(LogEventLevel.Debug, message);
    }

    public void Error(string message)
    {
        Logger.Write(LogEventLevel.Error, message);
    }

    public void Fatal(string message)
    {
        Logger.Write(LogEventLevel.Fatal, message);
    }

    public void Info(string message)
    {
        Logger.Write(LogEventLevel.Information, message);
    }

    public void Warn(string message)
    {
        Logger.Write(LogEventLevel.Warning, message);
    }

    public void Error(Exception ex)
    {
        Logger.Error(ex, "[Message]", ex.Message);
    }
}