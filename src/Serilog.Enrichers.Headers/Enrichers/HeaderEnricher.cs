using Serilog.Core;

#if NETFULL
using Serilog.Enrichers.Headers.Accessors;
#else
using Microsoft.AspNetCore.Http;
#endif
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Enrichers
{
    public class HeaderEnricher : ILogEventEnricher
    {
        private IHttpContextAccessor _contextAccessor;

        public HeaderEnricher()
        {
            _contextAccessor = new HttpContextAccessor();
        }

        public HeaderEnricher(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {

            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null)
                return;

            foreach (var key in HeaderConfig.Keys)
            {
                var value = httpContext.Request.Headers[key];
                if (string.IsNullOrEmpty(value)) continue;
                if (httpContext.Items[key] is LogEventProperty logEventProperty)
                {
                    logEvent.AddPropertyIfAbsent(logEventProperty);
                    return;
                }

                var headerValue = new LogEventProperty(key, new ScalarValue(value));
                httpContext.Items.Add(key, headerValue);

                logEvent.AddPropertyIfAbsent(headerValue);
            }
        }
    }
}
