namespace LafAPI.Data.Seeding
{
    using System;
    using System.Linq;

    using LafAPI.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class LafContextSeeder
    {
        public static void Seed(LafContext context, IServiceProvider serviceProvider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            Seed(context, roleManager);
        }

        public static void Seed(LafContext context, RoleManager<Role> roleManager)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }

            SeedRoles(roleManager);
        }

        private static void SeedRoles(RoleManager<Role> roleManager)
        {
            SeedRole(nameof(RoleType.Administrator), roleManager);
            SeedRole(nameof(RoleType.Moderator), roleManager);
            SeedRole(nameof(RoleType.Regular), roleManager);
        }

        private static void SeedRole(string roleName, RoleManager<Role> roleManager)
        {
            var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
            if (role == null)
            {
                var result = roleManager.CreateAsync(new Role(roleName)).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}