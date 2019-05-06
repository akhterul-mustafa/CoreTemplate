using Microsoft.AspNetCore.Builder;

namespace Dell.Solution.Service.Sample.Middleware
{
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationModule(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationModule>();
        }
    }
}
