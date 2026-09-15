namespace Huwiyati.Application.Organizations.Admins.Commands;

public class AssignAdminCommand
{
    public string NationalNumber { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    //public string EmployeeNumber { get; set; } = string.Empty;
}
