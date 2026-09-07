namespace Huwiyati.Domain.Common;

public abstract class BaseEntity
{
    // Use Guid.CreateVersion7() to generate a new GUID with a timestamp component mean mix two thing Guid and time
    public Guid Id { get; set; } = Guid.CreateVersion7();
}
