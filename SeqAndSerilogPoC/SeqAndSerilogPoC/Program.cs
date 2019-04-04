using LogCreate;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace SeqAndSerilogPoC
{
    internal class Program
    {
        // Fields ---------------------------------------------------------------------------------
        private static readonly Dictionary<string, string> _args = new Dictionary<string, string>();
        private static readonly string _logMessage = $"Hello, {AppHelper.ApplicationUser}!";
        private static LogEventLevel _logLevel = LogEventLevel.Information;
        private static ILogger _logger;

        // Constructors ---------------------------------------------------------------------------
        private static void Main(string[] args)
        {
            //get arguments
            ParseArgs(args);
            
            //configure logger
            SetLogLevel();
            Log.Logger = CareSourceLogger.Create(
                LogCreate.LogCreate.Create(
                    AppHelper.ConfigurationValues[ConfigKeys.LogUrl],
                    AppHelper.ConfigurationValues[ConfigKeys.LogKey],
                    _logLevel,
                    AppHelper.AppName,
                    AppHelper.AppVersion,
                    AppHelper.ConfigurationValues[ConfigKeys.Environment]));
            _logger = Log.ForContext<Program>();

            //this demonstrates the use of arguments to dynamically set minimum log level
            _logger.Information(_logMessage);
            _logger.Debug(_logMessage);
            //_logger.Information($"Hello, {AppHelper.ApplicationUser}! {@SomeClass}", someClassInstance); //Cant use C# string interpolation

            //this is to test varying scopes of logging context and different types of class destructuring 
            TestClass.Create().ContextAndDestructureTest();
            _logger.Information("This is a test to see what the context of this log is after coming out of a class that set a different one");

            //this is a test for verbose correlation and function timing
            TestClass.Create().CorrelationAndTimerTest();

            //this is a test of an exception
            try
            { throw new Exception("THIS IS A TEST"); }
            catch (Exception ex)
            { _logger.Error(ex, "This can be any destructured string"); }

            //exit
            Log.CloseAndFlush();
        }

        // Private Functions ----------------------------------------------------------------------
        private static void ParseArgs(IReadOnlyList<string> args)
        {
            for (var i = 0; i < args.Count; i += 2)
            { _args.Add(args[i], args[i + 1]); }
        }

        private static void SetLogLevel() =>
            _logLevel = _args.ContainsKey(AppHelper.ArgumentFlags[Arguments.Logging]) 
                ? Enum.TryParse(_args[AppHelper.ArgumentFlags[Arguments.Logging]], out LogEventLevel newLevel) 
                    ? newLevel
                    : _logLevel
                : _logLevel;
    }
}
