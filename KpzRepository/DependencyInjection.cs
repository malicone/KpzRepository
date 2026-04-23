using KpzRepository.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KpzRepository;

public static class DependencyInjection
{
    public static IServiceCollection AddKpzRepositoryFactory(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        var repoFactoryDescriptor = new ServiceDescriptor(
            typeof(IKpzRepositoryFactory),
            provider => new KpzRepositoryFactory(connectionString),
            ServiceLifetime.Transient);
        services.TryAdd(repoFactoryDescriptor);

        return services;
    }
}
