namespace Huwiyati.Application.Organizations.Employees.Commands;

public class UpdateEmployeeCommand
{
    public Guid EmployeeId { get; set; }
    public Guid NewBranchId { get; set; }
}
