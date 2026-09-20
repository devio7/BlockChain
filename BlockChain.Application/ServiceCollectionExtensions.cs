using BlockChain.Application.Common.Behaviors;
using BlockChain.Application.Features.Blockchain.CreateBlockchainEntry;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChain.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(CreateBlockchainEntryHandler).Assembly);

            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CreateBlockchainEntryHandler).Assembly);

        return services;
    }
}