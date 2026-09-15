namespace Huwiyati.Application.Common.Interfaces;

public interface IEmployeeNumberGenerator
{
    Task<string> GenerateEmployeeNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
}
