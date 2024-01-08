using DbUp;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void InfrastructureDependencyInjection(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHostedService<CleanupHostedService>();
            service.AddTransient<IItemRepository, ItemRepository>();

            var connectionString = configuration["MySecrets:PostgreConnection"] ?? throw new ArgumentNullException("Connection string was not found."); ;
            service.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));

            EnsureDatabase.For.PostgresqlDatabase(connectionString);
            var upgrader = DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToNowhere()
                    .Build();

            var result = upgrader.PerformUpgrade();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}