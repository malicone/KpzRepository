using Dapper;
using KpzRepository.Factory;
using KpzRepository.PostgreSql.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace KpzRepository.PostgreSql;

public static class DependencyInjection
{
    public static IServiceCollection AddKpzRepositoryPostgreSqlFactory(this IServiceCollection services, string? connectionString)
    {
        if(string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        SqlMapper.AddTypeHandler(new JsonbTypeHandler());

        var repoFactoryDescriptor = new ServiceDescriptor(
            typeof(IKpzRepositoryFactory),
            provider => new Factory.KpzRepositoryPostgreSqlFactory(connectionString),
            ServiceLifetime.Transient);
        services.Add(repoFactoryDescriptor);

        return services;
    }
}