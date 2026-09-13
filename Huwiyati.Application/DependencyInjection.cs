using Huwiyati.Application.Authentication.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register CQRS Handlers
            services.AddScoped<RegisterHandler>();
            services.AddScoped<VerifyOTPHandler>();
            services.AddScoped<LoginHandler>();
            services.AddScoped<ForgotPasswordHandler>();
            services.AddScoped<VerifyResetHandler>();
            services.AddScoped<ResetPasswordHandler>();
            services.AddScoped<VerifyDeviceHandler>();
            return services;
        }
    }
}
