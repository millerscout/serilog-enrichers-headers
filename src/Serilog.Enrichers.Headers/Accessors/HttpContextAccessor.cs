﻿#if NETFULL
using System.Web;

namespace Serilog.Enrichers.Headers.Accessors
{
    public interface IHttpContextAccessor
    {
        HttpContext HttpContext { get; }
    }

    internal class HttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext => HttpContext.Current;
    }
}
#endif