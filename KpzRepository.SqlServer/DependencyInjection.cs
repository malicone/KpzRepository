using KpzRepository.Factory;
using KpzRepository.SqlServer.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KpzRepository;

public static class DependencyInjection
{
    public static IServiceCollection AddKpzRepositorySqlServerFactory(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var repoFactoryDescriptor = new ServiceDescriptor(
            typeof(IKpzRepositoryFactory),
            provider => new KpzRepositorySqlServerFactory(connectionString),
            ServiceLifetime.Transient);
        services.TryAdd(repoFactoryDescriptor);

        return services;
    }
}
