namespace Huwiyati.Domain.Entities.Authentication;

using Huwiyati.Domain.Common;

public class UserDevice : BaseEntity
{
    public Guid UserId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string DeviceIdentifier { get; set; } = string.Empty;
    public string OperatingSystem { get; set; } = string.Empty;
    public bool IsTrusted { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}