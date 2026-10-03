namespace Huwiyati.Application.Notifications;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Notifications.EventHandlers.Authentication;
using Huwiyati.Application.Notifications.EventHandlers.Documents;
using Huwiyati.Application.Notifications.EventHandlers.Family;
using Huwiyati.Application.Notifications.EventHandlers.Organizations;
using Huwiyati.Application.Notifications.EventHandlers.Passport;
using Huwiyati.Application.Notifications.EventHandlers.Requests;
using Huwiyati.Domain.Events.Authentication;
using Huwiyati.Domain.Events.Documents;
using Huwiyati.Domain.Events.Family;
using Huwiyati.Domain.Events.Organizations;
using Huwiyati.Domain.Events.Passport;
using Huwiyati.Domain.Events.Requests;
using Microsoft.Extensions.DependencyInjection;

public static class NotificationEventHandlersDependencyInjection
{
    public static IServiceCollection AddNotificationEventHandlers(this IServiceCollection services)
    {
        // 1. Passport Event Handlers
        services.AddScoped<IDomainEventHandler<PassportIssuedEvent>, PassportIssuedEventHandler>();
        services.AddScoped<IDomainEventHandler<PassportRenewedEvent>, PassportRenewedEventHandler>();
        services.AddScoped<IDomainEventHandler<TravelRecordAddedEvent>, TravelRecordAddedEventHandler>();

        // 2. Authentication Event Handlers
        services.AddScoped<IDomainEventHandler<NewDeviceLoginAttemptEvent>, NewDeviceLoginAttemptEventHandler>();
        services.AddScoped<IDomainEventHandler<NewDeviceLoginSuccessEvent>, NewDeviceLoginSuccessEventHandler>();
        services.AddScoped<IDomainEventHandler<PasswordResetRequestedEvent>, PasswordResetRequestedEventHandler>();
        services.AddScoped<IDomainEventHandler<PasswordResetSuccessEvent>, PasswordResetSuccessEventHandler>();
        services.AddScoped<IDomainEventHandler<AccountDeactivatedEvent>, AccountDeactivatedEventHandler>();
        services.AddScoped<IDomainEventHandler<AccountReactivatedEvent>, AccountReactivatedEventHandler>();

        // 3. Organization (Admin & Employee) Event Handlers
        services.AddScoped<IDomainEventHandler<AdminAssignedEvent>, AdminAssignedEventHandler>();
        services.AddScoped<IDomainEventHandler<AdminUpdatedEvent>, AdminUpdatedEventHandler>();
        services.AddScoped<IDomainEventHandler<AdminRemovedEvent>, AdminRemovedEventHandler>();
        services.AddScoped<IDomainEventHandler<AdminRestoredEvent>, AdminRestoredEventHandler>();
        services.AddScoped<IDomainEventHandler<EmployeeAssignedEvent>, EmployeeAssignedEventHandler>();
        services.AddScoped<IDomainEventHandler<EmployeeUpdatedEvent>, EmployeeUpdatedEventHandler>();
        services.AddScoped<IDomainEventHandler<EmployeeDeactivatedEvent>, EmployeeDeactivatedEventHandler>();
        services.AddScoped<IDomainEventHandler<EmployeeActivatedEvent>, EmployeeActivatedEventHandler>();

        // 4. Document (National ID, Birth, Death) Event Handlers
        services.AddScoped<IDomainEventHandler<NationalIdCardIssuedEvent>, NationalIdCardIssuedEventHandler>();
        services.AddScoped<IDomainEventHandler<NationalIdCardRenewedEvent>, NationalIdCardRenewedEventHandler>();
        services.AddScoped<IDomainEventHandler<PersonDataUpdatedEvent>, PersonDataUpdatedEventHandler>();
        services.AddScoped<IDomainEventHandler<BirthCertificateIssuedEvent>, BirthCertificateIssuedEventHandler>();
        services.AddScoped<IDomainEventHandler<ChildDataUpdatedEvent>, ChildDataUpdatedEventHandler>();
        services.AddScoped<IDomainEventHandler<DeathCertificateIssuedEvent>, DeathCertificateIssuedEventHandler>();
        services.AddScoped<IDomainEventHandler<DeathCertificateUpdatedEvent>, DeathCertificateUpdatedEventHandler>();

        // 5. Family Event Handlers
        services.AddScoped<IDomainEventHandler<FamilyCardCreatedEvent>, FamilyCardCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<FamilyCardRenewedEvent>, FamilyCardRenewedEventHandler>();
        services.AddScoped<IDomainEventHandler<WifeAddedToFamilyEvent>, WifeAddedToFamilyEventHandler>();
        services.AddScoped<IDomainEventHandler<FamilyMemberStatusChangedEvent>, FamilyMemberStatusChangedEventHandler>();

        // 6. Request Event Handlers
        services.AddScoped<IDomainEventHandler<ServiceRequestCreatedEvent>, ServiceRequestCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<ServiceRequestStatusChangedEvent>, ServiceRequestStatusChangedEventHandler>();
        services.AddScoped<IDomainEventHandler<ServiceRequestCancelledEvent>, ServiceRequestCancelledEventHandler>();

        // 7. Expiry Document handlers
        services.AddScoped<IDomainEventHandler<DocumentsExpiringEvent>, NationalIdCardExpiringEventHandler>();
        services.AddScoped<IDomainEventHandler<DocumentsExpiringEvent>, PassportExpiringEventHandler>();
        services.AddScoped<IDomainEventHandler<DocumentsExpiringEvent>, FamilyCardExpiringEventHandler>();
        services.AddScoped<NationalIdCardExpiringEventHandler>();
        services.AddScoped<PassportExpiringEventHandler>();
        services.AddScoped<FamilyCardExpiringEventHandler>();

        return services;
    }
}
