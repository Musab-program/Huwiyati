namespace Huwiyati.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Infrastructure.Identity;
using Huwiyati.Infrastructure.Persistence;
using Huwiyati.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // 1. Register Data Protection services required by DefaultTokenProviders
        services.AddDataProtection();

        // 2. Register DbContext with SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // 3. Register ASP.NET Core Identity Core for Web APIs
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            // Password complexity rules
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;

            // User rules
            options.User.RequireUniqueEmail = true;

            // Lockout rules
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // 4. Register IdentityService Implementation
        services.AddTransient<IIdentityService, IdentityService>();

        // 5. Register IApplicationDbContext mapping to ApplicationDbContext
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // 6. Register TokenService Implementation
        services.AddTransient<ITokenService, TokenService>();

        // 7. Register EmployeeNumberGenerator Implementation
        services.AddScoped<IEmployeeNumberGenerator, EmployeeNumberGenerator>();

        // 8. Register EmailService Implementation
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
