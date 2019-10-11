namespace LafAPI.Web
{
    using LafAPI.Web.Infrastructure.Configurations;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        private readonly AllConfigurations allConfigurations;

        public Startup(IConfiguration configuration) =>
            this.allConfigurations = new AllConfigurations(configuration);

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) =>
            this.allConfigurations.Configure(app, env, loggerFactory);

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) =>
            this.allConfigurations.ConfigureServices(services);
    }
}