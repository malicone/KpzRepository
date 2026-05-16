using KpzRepository.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace KpzRepository.Sqlite;

public static class DependencyInjection
{
    public static IServiceCollection AddKpzRepositorySqliteFactory(this IServiceCollection services, string? connectionString)
    {
        if(string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        var repoFactoryDescriptor = new ServiceDescriptor(
            typeof(IKpzRepositoryFactory),
            provider => new Factory.KpzRepositorySqliteFactory(connectionString),
            ServiceLifetime.Transient);
        services.Add(repoFactoryDescriptor);

        return services;
    }
}