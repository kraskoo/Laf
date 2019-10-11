namespace LafAPI.Web.Infrastructure.Configurations
{
    using LafAPI.Data;
    using LafAPI.Data.Seeding;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class DataContextConfiguration : BaseConfiguration
    {
        public DataContextConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<LafContext>();

            if (env.IsDevelopment())
            {
                context.Database.Migrate();
            }

            LafContextSeeder.Seed(context, serviceScope.ServiceProvider);
        }

        public override void ConfigureServices(IServiceCollection services) =>
            services.AddDbContext<LafContext>(
                options => options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection")));
    }
}