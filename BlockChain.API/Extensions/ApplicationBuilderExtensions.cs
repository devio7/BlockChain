using BlockChain.API.Middlewares;

namespace BlockChain.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseLog(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LogMiddleware>();
    }
}