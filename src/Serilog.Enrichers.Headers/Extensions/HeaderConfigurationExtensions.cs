using Serilog.Configuration;
using Serilog.Enrichers;
using System;

namespace Serilog
{
    /// <summary>
    ///     Extension methods for setting up header enrichers <see cref="LoggerEnrichmentConfiguration"/>.
    /// </summary>
    public static class HeaderConfigurationExtensions
    {
        /// <summary>
        ///     Registers the header enricher to enrich logs with custom fields.
        /// </summary>
        /// <param name="enrichmentConfiguration"> The enrichment configuration. </param>
        /// <exception cref="ArgumentNullException"> enrichmentConfiguration </exception>
        /// <returns> The logger configuration so that multiple calls can be chained. </returns>
        public static LoggerConfiguration WithHeaders(this LoggerEnrichmentConfiguration enrichmentConfiguration, Func<string[]> headerKeys)
        {
            if (enrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(enrichmentConfiguration));

            HeaderConfig.Keys = headerKeys();

            return enrichmentConfiguration.With<HeaderEnricher>();
        }

    }
}