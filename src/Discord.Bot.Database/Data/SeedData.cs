
using System;
using System.Linq;
using Discord.Bot.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Serilog;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class SeedData
{
    /// <summary>
    /// Seed the database with admin user and credentials.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="secret">Plain-text password for the admin user.</param>
    /// <exception cref="ApplicationException">Unable to seed database with new user</exception>
    /// <exception cref="Exception">Something went wrong while saving the roles to the new user.</exception>
    public static async void Initialize(IServiceProvider serviceProvider, string secret)
    {
        Log.Verbose("Initializing the seed data...");
        AppDBContext context = serviceProvider.GetService<AppDBContext>();

        // Create roles if they don't exist
        string[] roles = new string[] { "Administrator" };
        var roleStore = new RoleStore<IdentityRole>(context);
        foreach (string role in roles)
        {
            Log.Verbose("Checking the {0} role", role);
            if (!context.Roles.Any(r => r.Name == role))
            {
                Log.Verbose("Creating the {0} role", role);
                await roleStore.CreateAsync(new IdentityRole() { Name = role, NormalizedName = role.ToUpperInvariant() });
            }
        }

        var username = "Administrator";
        IdentityUser identityUser = null;
        Log.Verbose("Checking users...");
        if (!context.Users.Any(u => u.UserName == username))
        {
            // Create a sample user
            identityUser = new IdentityUser
            {
                UserName = "Administrator",
                Email = "admin@example.com",
                // Set other properties as needed
            };

            Log.Verbose("No users exist, creating admin user.");
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var hashedPassword = passwordHasher.HashPassword(identityUser, secret);
            identityUser.PasswordHash = hashedPassword;

            var userStore = new UserStore<IdentityUser>(context);
            Log.Information($"Seeding new admin user with UserName: {identityUser.UserName} and Password: {secret}");
            var result = await userStore.CreateAsync(identityUser);

            if (result.Errors.Any())
            {
                var msg = $"Unable to seed database with new user {identityUser.UserName} {string.Join(",", result.Errors)}";
                Log.Error(msg);
                throw new ApplicationException(msg);
            }
            else
            {
                Log.Information("Admin user seeded successfully!");
            }
        }
        else
        {
            identityUser = await context.Users.FirstAsync(u => u.UserName == username);
        }

        var nm = serviceProvider.GetService<UserManager<IdentityUser>>();

        if (!(await nm.GetRolesAsync(identityUser)).Any(r => r == username))
        {

            // Assign roles to the user
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var identityResult = await userManager.AddToRolesAsync(identityUser, roles);


            if (!identityResult.Succeeded)
            {
                Log.Error("An error occurred..\n{0}", string.Join("\n", identityResult.Errors.Select(e => e.Description)));
                throw new Exception("Something went wrong while saving the roles to the new user.");
            }
        }

        await context.SaveChangesAsync();
    }

}
