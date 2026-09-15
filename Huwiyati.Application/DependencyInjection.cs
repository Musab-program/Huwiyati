using Huwiyati.Application.Authentication.Commands;
using Huwiyati.Application.Organizations.Admins.Commands;
using Huwiyati.Application.Organizations.Admins.Queries;
using Huwiyati.Application.Organizations.Branches.Commands;
using Huwiyati.Application.Organizations.Branches.Queries;
using Huwiyati.Application.Organizations.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Huwiyati.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Authentication Handlers
            services.AddScoped<RegisterHandler>();
            services.AddScoped<VerifyOTPHandler>();
            services.AddScoped<LoginHandler>();
            services.AddScoped<ForgotPasswordHandler>();
            services.AddScoped<VerifyResetHandler>();
            services.AddScoped<ResetPasswordHandler>();
            services.AddScoped<VerifyDeviceHandler>();
            services.AddScoped<RemoveDeviceHandler>();
            services.AddScoped<DeactivateAccountHandler>();
            services.AddScoped<ReactivateAccountHandler>();
            services.AddScoped<RequestAccountOtpHandler>();

            // Register Organization Branch Handlers
            services.AddScoped<CreateBranchHandler>();
            services.AddScoped<UpdateBranchHandler>();
            services.AddScoped<DeleteBranchHandler>();
            services.AddScoped<RestoreBranchHandler>();
            services.AddScoped<GetOrganizationsQueryHandler>();
            services.AddScoped<GetBranchesQueryHandler>();
            services.AddScoped<GetBranchByIdHandler>();

            // Register Organization Admin Handlers
            services.AddScoped<AssignAdminHandler>();
            services.AddScoped<GetAdminsHandler>();
            services.AddScoped<GetAdminByIdHandler>();
            services.AddScoped<UpdateAdminHandler>();
            services.AddScoped<RemoveAdminHandler>();
            services.AddScoped<RestoreAdminHandler>();

            // Register Organization Employee Handlers
            services.AddScoped<Huwiyati.Application.Organizations.Employees.Commands.AssignEmployeeHandler>();
            services.AddScoped<Huwiyati.Application.Organizations.Employees.Commands.UpdateEmployeeHandler>();
            services.AddScoped<Huwiyati.Application.Organizations.Employees.Queries.GetEmployeesHandler>();
            services.AddScoped<Huwiyati.Application.Organizations.Employees.Queries.GetEmployeeByIdHandler>();
            services.AddScoped<Huwiyati.Application.Organizations.Employees.Commands.DeactivateEmployeeHandler>();
            services.AddScoped<Huwiyati.Application.Organizations.Employees.Commands.ActivateEmployeeHandler>();

            return services;
        }
    }
}
