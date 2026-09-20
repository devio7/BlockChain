namespace BlockChain.API.Middlewares;

public sealed class LogMiddleware : IMiddleware
{
    private readonly ILogger<LogMiddleware> _logger;

    public LogMiddleware(ILogger<LogMiddleware> logger)
    {
       _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("Request Executing....");

        await next(context);

        _logger.LogInformation("Request Executed....");
    }
}