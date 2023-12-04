using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using TechChallenge.Persistence.Repositories;
using TechChallenge.Persistence.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(ConnectionString.SettingsKey);

            services.AddSingleton(new ConnectionString(connectionString));
            services.AddDbContext<EFContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<EFContext>());
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<EFContext>());

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
