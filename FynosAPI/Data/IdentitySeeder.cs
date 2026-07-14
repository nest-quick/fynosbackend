using FynosAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace FynosAPI.Data;

public static class IdentitySeeder
{
    private const string AdminRole = "Admin";

    public static async Task SeedAdminAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        var adminEmail =
            configuration["IdentitySeeder:AdminEmail"];

        var adminPassword =
            configuration["IdentitySeeder:AdminPassword"];

        if (string.IsNullOrWhiteSpace(adminEmail))
        {
            throw new InvalidOperationException(
                "IdentitySeeder:AdminEmail is missing.");
        }

        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException(
                "IdentitySeeder:AdminPassword is missing.");
        }

        await EnsureAdminRoleExistsAsync(roleManager);

        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult =
                await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createResult.Errors
                        .Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to create admin user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(
                adminUser,
                AdminRole))
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    adminUser,
                    AdminRole);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors
                        .Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to assign admin role: {errors}");
            }
        }
    }

    private static async Task EnsureAdminRoleExistsAsync(
        RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.RoleExistsAsync(AdminRole))
        {
            return;
        }

        var result =
            await roleManager.CreateAsync(
                new IdentityRole(AdminRole));

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors
                    .Select(error => error.Description));

            throw new InvalidOperationException(
                $"Failed to create Admin role: {errors}");
        }
    }
}
