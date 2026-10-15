namespace Huwiyati.Application.Requests.Commands;

/// <summary>
/// Command carrying client parameters to initiate a new civil service request.
/// </summary>
public class CreateServiceRequestCommand
{
    /// <summary>
    /// Gets or sets an optional person identifier when an employee submits on behalf of a citizen.
    /// </summary>
    public Guid? PersonId { get; set; }

    /// <summary>
    /// Gets or sets an optional national identification number when an employee submits on behalf of a citizen.
    /// </summary>
    public string? NationalNumber { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the selected service type.
    /// </summary>
    public Guid ServiceTypeId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the processing organization branch.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets dynamic payload object containing specific service-dependent form data.
    /// </summary>
    public object? RequestDataJson { get; set; }
}
