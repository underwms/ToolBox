using Serilog;
using Serilog.Core;
using System.Collections.Generic;

namespace LogCreate
{
    public class CareSourceLogger
    {
        // Public Functions -----------------------------------------------------------------------
        public static ILogger Create(LogCreate settings) =>
            (string.IsNullOrWhiteSpace(settings.SeqUrl) || string.IsNullOrWhiteSpace(settings.SeqApiKey))
                ? null
                : new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(new LoggingLevelSwitch(settings.MinimumLogLevel))
                    .WriteTo.Seq(serverUrl: settings.SeqUrl,
                                 apiKey: settings.SeqApiKey)
                    .Enrich.With(SeqLogEventEnricher.Create(
                        CreateEnrichedPropertiesDictionary(
                            settings.ApplicationName,
                            settings.ApplicationVersion,
                            settings.ApplicationEnvironment,
                            settings.EnrichedProperties)
                    ))
                    .CreateLogger();

        // Private Functions ----------------------------------------------------------------------
        private static IDictionary<string, string> CreateEnrichedPropertiesDictionary(
            string applicationName,
            string applicationVersion,
            string applicationEnvironment,
            IDictionary<string, string> enrichedProperties)
        {
            var baseDictionary = new Dictionary<string, string>()
            {
                [LogHelper.APPLICATION_NAME] = applicationName,
                [LogHelper.APPLICATION_VERSION] = applicationVersion,
                [LogHelper.APPLICATION_ENVIRONMENT] = applicationEnvironment,
            };

            if (enrichedProperties is null) return baseDictionary;

            foreach (var kvp in enrichedProperties)
            {
                if (baseDictionary.ContainsKey(kvp.Key)) continue;
                baseDictionary.Add(kvp.Key, LogHelper.StringCheck(kvp.Value));
            }

            return baseDictionary;
        }
    }
}
