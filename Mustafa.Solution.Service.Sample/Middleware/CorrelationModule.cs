using Dell.Solution.Service.Sample.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Dell.Solution.Service.Sample.Middleware
{
    public class CorrelationModule
    {
        private readonly RequestDelegate _next;

        public CorrelationModule(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Do something with context near the beginning of request processing.
            string correlationId = context.Request.Headers[Constants.CorrelationHeader];
            if (String.IsNullOrEmpty(correlationId))
            {
                context.Request.Headers.Add(Constants.CorrelationHeader, Guid.NewGuid().ToString());
            }
            await _next.Invoke(context);

            // Clean up.
        }
    }
}
