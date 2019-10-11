namespace LafAPI.Web.Infrastructure.Configurations
{
    using LafAPI.Data;
    using LafAPI.Data.Common.Repositories;
    using LafAPI.Data.Models;
    using LafAPI.Data.Repositories;
    using LafAPI.Services.Messaging;
    using LafAPI.Web.Infrastructure.Interfaces;
    using LafAPI.Web.Infrastructure.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class IOCConfiguration : BaseConfiguration
    {
        public IOCConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserFriendService, UserFriendService>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISmsSender, NullMessageSender>();

            // Identity stores
            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddTransient<IRoleStore<Role>, RoleStore>();
        }
    }
}