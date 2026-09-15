namespace Huwiyati.Application.Organizations.Employees.DTOs;

public class EmployeeDto
{
    public Guid EmployeeId { get; set; }
    public Guid UserId { get; set; }
    public string NationalNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
