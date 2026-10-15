namespace Huwiyati.Domain.Enums;

/// <summary>
/// Specifies the kinship relationship of an individual relative to the primary Head of Family.
/// </summary>
public enum RelationshipType
{
    /// <summary>
    /// The primary head and holder of the family card record.
    /// </summary>
    Head = 1,

    /// <summary>
    /// Legal spouse/wife tied through a certified marriage contract.
    /// </summary>
    Wife = 2,

    /// <summary>
    /// Biological or legally recognized male child.
    /// </summary>
    Son = 3,

    /// <summary>
    /// Biological or legally recognized female child.
    /// </summary>
    Daughter = 4
}
