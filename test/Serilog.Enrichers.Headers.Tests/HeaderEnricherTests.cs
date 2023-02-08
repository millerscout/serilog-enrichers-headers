using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Events;
using System;
using System.Net;
using Xunit;

namespace Serilog.Enrichers.Headers.Tests
{
    public class HeaderEnricherTests
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HeaderEnricherTests()
        {
            var httpContext = new DefaultHttpContext();
            _contextAccessor = Substitute.For<IHttpContextAccessor>();
            _contextAccessor.HttpContext.Returns(httpContext);
        }

        [Fact]
        public void When_Enrich_Log_Event_With_HeaderEnricher_From_HttpContext_Items()
        {
            var value = Guid.NewGuid().ToString();
            _contextAccessor.HttpContext.Request.Headers["traceid"] = value;
            var headerEnricher = new HeaderEnricher(_contextAccessor);

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(headerEnricher)
                .Enrich.WithHeaders(() =>
                {
                    return new string[] { "traceid" };
                })
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"some information log");


            Assert.NotNull(evt);
            Assert.True(evt.Properties.ContainsKey("traceid"));
            Assert.Equal(value, evt.Properties["traceid"].ToString());
        }

    }
}