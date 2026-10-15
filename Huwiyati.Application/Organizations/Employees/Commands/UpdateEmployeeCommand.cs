namespace Huwiyati.Application.Organizations.Employees.Commands;

/// <summary>
/// Command to transfer or update an employee's branch assignment.
/// </summary>
public class UpdateEmployeeCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the employee to update.
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the target destination branch.
    /// </summary>
    public Guid NewBranchId { get; set; }
}
