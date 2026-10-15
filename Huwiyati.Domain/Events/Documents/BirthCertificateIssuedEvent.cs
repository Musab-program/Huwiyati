namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when a birth certificate has been officially issued for a child.
/// </summary>
public class BirthCertificateIssuedEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the father.
    /// </summary>
    public Guid FatherPersonId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the registered child.
    /// </summary>
    public string ChildName { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="BirthCertificateIssuedEvent"/> class.
    /// </summary>
    /// <param name="fatherPersonId">The unique identifier of the father.</param>
    /// <param name="childName">The full name of the registered child.</param>
    public BirthCertificateIssuedEvent(Guid fatherPersonId, string childName)
    {
        FatherPersonId = fatherPersonId;
        ChildName = childName;
    }
}
