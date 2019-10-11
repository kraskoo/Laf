namespace LafAPI.Web.Infrastructure.Configurations
{
    using LafAPI.Web.Infrastructure.Interfaces;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public abstract class BaseConfiguration : IAspNetCoreConfiguration
    {
        protected BaseConfiguration(IConfiguration configuration) =>
            this.Configuration = configuration;

        protected IConfiguration Configuration { get; set; }

        public abstract void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);

        public abstract void ConfigureServices(IServiceCollection services);
    }
}