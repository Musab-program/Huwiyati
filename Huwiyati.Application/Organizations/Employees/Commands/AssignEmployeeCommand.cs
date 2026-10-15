namespace Huwiyati.Application.Organizations.Employees.Commands;

/// <summary>
/// Command to assign a citizen as an employee in the administrator's branch.
/// </summary>
public class AssignEmployeeCommand
{
    /// <summary>
    /// Gets or sets the national identification number of the citizen to be assigned as employee.
    /// </summary>
    public string NationalNumber { get; set; } = string.Empty;
}
