namespace LafAPI.Web.Infrastructure.Configurations
{
    using System.Collections.Generic;

    using LafAPI.Web.Infrastructure.Interfaces;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class AllConfigurations
    {
        private readonly IReadOnlyCollection<IAspNetCoreConfiguration> configurations;

        public AllConfigurations(IConfiguration configuration)
            => this.configurations = new List<IAspNetCoreConfiguration>
                                      {
                                          new EnvironmentConfiguration(configuration),
                                          new IOCConfiguration(configuration),
                                          new DataContextConfiguration(configuration),
                                          new IdentityConfiguration(configuration),
                                          new ServerConfiguration(configuration)
                                      };

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            foreach (var configuration in this.configurations)
            {
                configuration.Configure(app, env, loggerFactory);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var configuration in this.configurations)
            {
                configuration.ConfigureServices(services);
            }
        }
    }
}