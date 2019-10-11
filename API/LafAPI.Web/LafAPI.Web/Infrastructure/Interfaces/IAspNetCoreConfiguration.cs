namespace LafAPI.Web.Infrastructure.Interfaces
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public interface IAspNetCoreConfiguration
    {
        void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);

        void ConfigureServices(IServiceCollection services);
    }
}