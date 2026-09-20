using BlockChain.Application.ExternalServices.Abstract;
using BlockChain.Domain.Interfaces;
using BlockChain.Infrastructure.ExternalServices.BlockCypher;
using BlockChain.Infrastructure.Persistence;
using BlockChain.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlockChain.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("MainConnection") ?? "IC.BlockChain.db";

        services.AddDbContext<BlockchainDbContext>(options => options.UseSqlite(connectionString));

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBlockchainRepository, BlockchainRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BlockchainDbContext>());

        return services;
    }

    public static IServiceCollection AddBlockCypherClient(this IServiceCollection services)
    {
        services.AddOptions<BlockCypherClientOptions>().Configure<IConfiguration>((options, config) =>
        {
            config.GetSection("BlockCypherApi").Bind(options);
        });

        services.AddHttpClient(BlockCypherConstants.HttpClientName, (serviceProvider, client) =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<BlockCypherClientOptions>>()
                .Value;

            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddScoped<IBlockCypherClient, BlockCypherClient>();

        return services;
    }
}