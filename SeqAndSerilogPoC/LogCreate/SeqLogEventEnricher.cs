using Serilog.Core;
using Serilog.Events;
using System.Collections.Generic;
using System.Linq;

namespace LogCreate
{
    public class SeqLogEventEnricher : ILogEventEnricher
    {
        // Fields ---------------------------------------------------------------------------------
        private readonly Dictionary<string, string> _enrichedProperties = new Dictionary<string, string>();

        // Properties -----------------------------------------------------------------------------
        public IDictionary<string, string> EnrichedProperties => _enrichedProperties;

        // Constructors ---------------------------------------------------------------------------
        internal SeqLogEventEnricher(IDictionary<string, string> enrichedProperties) =>
            enrichedProperties.ToList()
                .ForEach(kvp => _enrichedProperties[kvp.Key] = kvp.Value);  //note: this will overwrite any keys already present in the dictionary

        // Public Functions -----------------------------------------------------------------------
        public static SeqLogEventEnricher Create(IDictionary<string, string> enrichedProperties) =>
            new SeqLogEventEnricher(enrichedProperties);

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (var kvp in _enrichedProperties)
            {
                logEvent.AddPropertyIfAbsent(
                    propertyFactory.CreateProperty(kvp.Key, kvp.Value));
            }
        }
    }
}
