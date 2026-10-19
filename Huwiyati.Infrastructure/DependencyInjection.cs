namespace Huwiyati.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Infrastructure.BackgroundJobs;
using Huwiyati.Infrastructure.Identity;
using Huwiyati.Infrastructure.Persistence;
using Huwiyati.Infrastructure.Services;

/// <summary>
/// Provides extension methods for registering infrastructure services into the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all infrastructure services, including database contexts, identity, background jobs, and utility services.
    /// </summary>
    /// <param name="services">The service collection to append registrations to.</param>
    /// <param name="configuration">The application configuration root.</param>
    /// <returns>The modified service collection.</returns>
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

        // 9. Register DocumentNumberGenerator Implementation
        services.AddScoped<IDocumentNumberGenerator, DocumentNumberGenerator>();

        // 10. Register RequestNumberGenerator Implementation
        services.AddScoped<IRequestNumberGenerator, RequestNumberGenerator>();

        // 11. Register ServicePayloadValidator Implementation
        services.AddScoped<IServicePayloadValidator, ServicePayloadValidator>();

        // 11.1 Register ServiceRequestFulfillmentService Implementation
        services.AddScoped<IServiceRequestFulfillmentService, ServiceRequestFulfillmentService>();

        // 11.2 Register BranchAdminValidator Implementation
        services.AddScoped<IBranchAdminValidator, BranchAdminValidator>();

        // 12. Register QRCodeService Implementation
        services.AddTransient<IQRCodeService, QRCodeService>();

        // 13. Register FileStorageService Implementation
        services.AddTransient<IFileStorageService, FileStorageService>();

        // 14. Register Current User Service
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // 15. Register Background Jobs
        services.AddHostedService<DocumentExpirationMonitorJob>();
        services.AddHostedService<VerificationCodeCleanupJob>();

        return services;
    }
}
