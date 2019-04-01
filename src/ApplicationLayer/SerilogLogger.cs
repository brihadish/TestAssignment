using System;
using EventFlow.Logs;
using Serilog;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace ApplicationLayer
{
    public sealed class SerilogLogger : EventFlow.Logs.Log
    {
        private readonly LogLevel _logLevel;
        private readonly ILogger _logger;

        public SerilogLogger(LogLevel logLevel, string azureApplicationInsightsInstrumentationKey)
        {
            var loggingConfiguration = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug();
            if (!string.IsNullOrWhiteSpace(azureApplicationInsightsInstrumentationKey))
            {
                loggingConfiguration = 
                    loggingConfiguration.WriteTo.ApplicationInsights(azureApplicationInsightsInstrumentationKey, new TraceTelemetryConverter());
            }

            _logLevel = logLevel;
            _logger = loggingConfiguration.CreateLogger();
        }

        protected override bool IsVerboseEnabled => (_logLevel & LogLevel.Verbose) == LogLevel.Verbose;

        protected override bool IsInformationEnabled => (_logLevel & LogLevel.Information) == LogLevel.Information;

        protected override bool IsDebugEnabled => (_logLevel & LogLevel.Debug) == LogLevel.Debug;

        public override void Write(LogLevel logLevel, string format, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _logger.Debug(format, args);
                    break;

                case LogLevel.Verbose:
                    _logger.Verbose(format, args);
                    break;

                case LogLevel.Information:
                    _logger.Information(format, args);
                    break;

                case LogLevel.Warning:
                    _logger.Warning(format, args);
                    break;

                case LogLevel.Error:
                    _logger.Error(format, args);
                    break;

                case LogLevel.Fatal:
                    _logger.Fatal(format, args);
                    break;
            }
        }

        public override void Write(LogLevel logLevel, Exception exception, string format, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _logger.Debug(exception, format, args);
                    break;

                case LogLevel.Verbose:
                    _logger.Verbose(exception, format, args);
                    break;

                case LogLevel.Information:
                    _logger.Information(exception, format, args);
                    break;

                case LogLevel.Warning:
                    _logger.Warning(exception, format, args);
                    break;

                case LogLevel.Error:
                    _logger.Error(exception, format, args);
                    break;

                case LogLevel.Fatal:
                    _logger.Fatal(exception, format, args);
                    break;
            }
        }
    }
}
