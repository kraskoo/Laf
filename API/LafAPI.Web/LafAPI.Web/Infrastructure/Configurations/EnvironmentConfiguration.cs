namespace LafAPI.Web.Infrastructure.Configurations
{
    using System.Net;
    using System.Reflection;
    using LafAPI.Common.Mapping;
    using LafAPI.Web.Infrastructure.Common;
    using LafAPI.Web.Infrastructure.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    public class EnvironmentConfiguration : BaseConfiguration
    {
        public EnvironmentConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            AutoMapperConfig.RegisterMappings(typeof(Startup).GetTypeInfo().Assembly);
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler(
                    application =>
                        {
                            application.Run(
                                async context =>
                                    {
                                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                        context.Response.ContentType = ApplicationConstants.JsonContentType;
                                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                                        if (ex != null)
                                        {
                                            await context.Response.WriteAsync(
                                                JsonConvert.SerializeObject(
                                                    new ExceptionHandlerFeatureAppModel
                                                        {
                                                            Message = ex.Error?.Message,
                                                            StackTrace = ex.Error?.StackTrace
                                                        })).ConfigureAwait(false);
                                        }
                                    });
                        });
            }
        }

        public override void ConfigureServices(IServiceCollection services)
        {
        }
    }
}