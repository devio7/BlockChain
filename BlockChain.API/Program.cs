using System.Text.Json.Serialization;
using BlockChain.API.Exceptions;
using BlockChain.API.Extensions;
using BlockChain.API.Middlewares;
using BlockChain.Application;
using BlockChain.Infrastructure;
using BlockChain.Infrastructure.Persistence;
using Serilog;

namespace BlockChain.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddSingleton(TimeProvider.System);

            builder.Services.AddMediatR();
            builder.Services.AddValidators();
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddBlockCypherClient();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.AddOpenApi();
            builder.Services.AddTransient<LogMiddleware>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            builder.Services.AddHealthChecks().AddDbContextCheck<BlockchainDbContext>(name: "sqlite", tags: ["database"]);

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            // Create Sqlite db if it is not exists.
            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<BlockchainDbContext>();

            db.Database.EnsureCreated();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "BlochChain API");
                });
            }

            app.UseExceptionHandler();
            app.UseLog();
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();
            app.MapHealthChecks("/health");
            app.MapControllers();
            app.Run();
        }
    }
}
