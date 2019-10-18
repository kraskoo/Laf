namespace LafAPI.Web
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Debug;

    using static LafAPI.Web.Infrastructure.Common.ApplicationConstants;

    public class Program
    {
        public static void Main() =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(CurrentDirectory)
                .UseIISIntegration()
                .ConfigureAppConfiguration(
                    (builderContext, config) =>
                        {
                            builderContext.Configuration = config.SetBasePath(ConfigPath)
                                .AddJsonFile(Path.Combine(ConfigPath, GetSettingsFilename(builderContext)), false, true)
                                .Build();
                        })
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                    logging.AddFilter(nameof(System), LogLevel.Debug)
                        .AddFilter<DebugLoggerProvider>(nameof(Microsoft), LogLevel.Trace))
                .ConfigureKestrel((context, options) =>
                    {
                        var httpPort = int.Parse(context.Configuration["Kestrel:Ports:http"]);
                        options.ListenAnyIP(httpPort);
                        var httpsPort = int.Parse(context.Configuration["Kestrel:Ports:https"]);
                        options.ListenAnyIP(
                            httpsPort,
                            opts =>
                                {
                                    opts.UseHttps();
                                });
                        options.Limits.MaxConcurrentConnections = 1000;
                        options.Limits.MaxConcurrentUpgradedConnections = 1000;
                        options.Limits.MaxRequestBodySize = 10 * 1024;
                        options.Limits.MinRequestBodyDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
                        options.Limits.MinResponseDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
                        options.Limits.KeepAliveTimeout = TimeSpan.FromDays(1);
                        options.Limits.RequestHeadersTimeout = TimeSpan.FromDays(1);
                        options.Limits.Http2.InitialConnectionWindowSize = 131072;
                    })
                .Build()
                .Run();

        private static string GetSettingsFilename(WebHostBuilderContext context) =>
            context.HostingEnvironment.EnvironmentName switch
            {
                Development => DevelopmentSettings,
                Staging => StagingSettings,
                _ => DefaultSettings,
            };
    }
}