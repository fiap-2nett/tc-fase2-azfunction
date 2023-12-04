using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using TechChallenge.Persistence.Infrastructure;

namespace TechChallenge.Persistence.Core.Primitives
{
    internal sealed class EFContextFactory : IDesignTimeDbContextFactory<EFContext>
    {
        #region IDesignTimeDbContextFactory Members

        public EFContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EFContext>();
            var connectionString = configuration.GetConnectionString(ConnectionString.SettingsKey);

            optionsBuilder.UseSqlServer(connectionString);

            return new EFContext(optionsBuilder.Options);
        }

        #endregion
    }
}
