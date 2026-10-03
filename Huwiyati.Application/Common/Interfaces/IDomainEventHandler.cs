namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Domain.Common;

// Generic interface for handling Domain Events across the Application layer
public interface IDomainEventHandler<in TEvent> where TEvent : BaseEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
} 
  
