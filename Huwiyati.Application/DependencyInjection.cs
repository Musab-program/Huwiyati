using Huwiyati.Application.Authentication.Commands;
using Huwiyati.Application.Documents.BirthCertificate.Commands;
using Huwiyati.Application.Documents.BirthCertificate.Queries;
using Huwiyati.Application.Documents.DeathCertificate.Commands;
using Huwiyati.Application.Documents.DeathCertificate.Queries;
using Huwiyati.Application.Documents.NationalIdCard.Commands;
using Huwiyati.Application.Documents.NationalIdCard.Queries;
using Huwiyati.Application.Family.Commands;
using Huwiyati.Application.Family.Queries;
using Huwiyati.Application.Organizations.Admins.Commands;
using Huwiyati.Application.Organizations.Admins.Queries;
using Huwiyati.Application.Organizations.Branches.Commands;
using Huwiyati.Application.Organizations.Branches.Queries;
using Huwiyati.Application.Organizations.Employees.Commands;
using Huwiyati.Application.Organizations.Employees.Queries;
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
            services.AddScoped<AssignEmployeeHandler>();
            services.AddScoped<UpdateEmployeeHandler>();
            services.AddScoped<GetEmployeesHandler>();
            services.AddScoped<GetEmployeeByIdHandler>();
            services.AddScoped<DeactivateEmployeeHandler>();
            services.AddScoped<ActivateEmployeeHandler>();

            // Register National ID Card Handlers
            services.AddScoped<IssueNationalIdCardHandler>();
            services.AddScoped<RenewNationalIdCardHandler>();
            services.AddScoped<UpdatePersonDataHandler>();
            services.AddScoped<GetNationalIdCardsHandler>();
            services.AddScoped<GetNationalIdCardByIdHandler>();
            services.AddScoped<GetPersonNationalIdCardHistoryHandler>();

            // Register Birth Certificate Handlers
            services.AddScoped<IssueBirthCertificateHandler>();
            services.AddScoped<UpdateChildDataHandler>();
            services.AddScoped<GetBirthCertificateByIdHandler>();
            services.AddScoped<GetBirthCertificatesByFatherNationalNumberHandler>();
            services.AddScoped<GetAllBirthCertificatesHandler>();

            // Register Death Certificate Handlers
            services.AddScoped<IssueDeathCertificateHandler>();
            services.AddScoped<UpdateDeathCertificateHandler>();
            services.AddScoped<GetAllDeathCertificatesHandler>();
            services.AddScoped<GetDeathCertificateByIdHandler>();

            // Register Family Handlers
            services.AddScoped<CreateFamilyCardHandler>();
            services.AddScoped<RenewFamilyCardHandler>();
            services.AddScoped<AddWifeHandler>();
            services.AddScoped<UpdateFamilyMemberStatusHandler>();
            services.AddScoped<GetActiveFamiliesHandler>();
            services.AddScoped<GetFamilyByIdHandler>();
            services.AddScoped<GetFamilyHistoryByFamilyNumberHandler>();

            return services;
        }
    }
}
