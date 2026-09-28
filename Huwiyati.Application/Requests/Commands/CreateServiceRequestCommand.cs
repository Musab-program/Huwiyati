namespace Huwiyati.Application.Requests.Commands;

// Command carrying client request parameters to initiate a new ServiceRequest
public class CreateServiceRequestCommand
{
    // Optional PersonId (Used when Employee submits on behalf of a citizen)
    public Guid? PersonId { get; set; }

    // Optional NationalNumber (Used when Employee submits on behalf of a citizen)
    public string? NationalNumber { get; set; }

    // Selected Service Type Identifier
    public Guid ServiceTypeId { get; set; }

    // Required processing Branch identifier selected by applicant
    public Guid BranchId { get; set; }

    // Dynamic payload sent directly from client app
    public object? RequestDataJson { get; set; }
}
