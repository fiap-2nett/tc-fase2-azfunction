using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;

using TechChallenge.Persistence;
using TechChallenge.FunctionApp.Extensions;

[assembly: WebJobsStartup(typeof(DbInitializationService), nameof(DbInitializationService))]
namespace TechChallenge.FunctionApp.Extensions
{
    internal sealed class DbInitializationService : IWebJobsStartup
    {
        #region IWebJobsStartup Members

        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<DbInitializerConfigProvider>();
        }

        #endregion
    }

    [Extension(nameof(DbInitializerConfigProvider))]
    internal sealed class DbInitializerConfigProvider : IExtensionConfigProvider
    {
        #region Read-Only Fields

        private readonly IServiceScopeFactory _serviceScopeFactory;

        #endregion

        #region Constructors

        public DbInitializerConfigProvider(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        #endregion

        #region IExtensionConfigProvider Members

        public void Initialize(ExtensionConfigContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<EFContext>();

            dbContext.Database.EnsureCreated();
        }

        #endregion
    }
}
