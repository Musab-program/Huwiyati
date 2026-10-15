namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the legal validity status of a registered marriage contract.
/// </summary>
public enum MarriageStatus
{
    /// <summary>
    /// Marriage contract is legally active and currently binding.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Marriage contract was legally terminated by divorce.
    /// </summary>
    Divorced = 2
}
