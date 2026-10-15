namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Domain.Common;

/// <summary>
/// Defines a contract for asynchronous domain event consumers across the application layer.
/// </summary>
/// <typeparam name="TEvent">The specific type of <see cref="BaseEvent"/> to handle.</typeparam>
public interface IDomainEventHandler<in TEvent> where TEvent : BaseEvent
{
    /// <summary>
    /// Handles the specified domain event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
} 
  
