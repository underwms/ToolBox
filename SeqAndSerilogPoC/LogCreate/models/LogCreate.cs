using Serilog.Events;
using System.Collections.Generic;

namespace LogCreate
{
    public class LogCreate
    {
        // Properties -----------------------------------------------------------------------------
        public string SeqUrl;
        public string SeqApiKey;
        public LogEventLevel MinimumLogLevel;
        public string ApplicationName;
        public string ApplicationVersion;
        public string ApplicationEnvironment;
        public IDictionary<string, string> EnrichedProperties;

        // Constructors ---------------------------------------------------------------------------
        internal LogCreate(
            string seqUrl,
            string seqApiKey,
            LogEventLevel minimumLogLevel,
            string applicationName,
            string applicationVersion,
            string applicationEnvironment,
            IDictionary<string, string> enrichedProperties)
        {
            SeqUrl = seqUrl;
            SeqApiKey = seqApiKey;
            MinimumLogLevel = minimumLogLevel;
            ApplicationName = applicationName;
            ApplicationVersion = applicationVersion;
            ApplicationEnvironment = applicationEnvironment;
            EnrichedProperties = enrichedProperties;
        }

        // Public Functions -----------------------------------------------------------------------
        public static LogCreate Create(
            string seqUrl = "",
            string seqApiKey = "",
            LogEventLevel minimumLogLevel = LogEventLevel.Information,
            string applicationName = LogHelper.UNKNOWN,
            string applicationVersion = LogHelper.UNKNOWN,
            string applicationEnvironment = LogHelper.UNKNOWN,
            IDictionary<string, string> enrichedProperties = null) =>
        new LogCreate(
            seqUrl,
            seqApiKey,
            minimumLogLevel,
            LogHelper.StringCheck(applicationName),
            LogHelper.StringCheck(applicationVersion),
            LogHelper.StringCheck(applicationEnvironment),
            enrichedProperties);
    }
}
