using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public TestController(ApplicationDbContext context , IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        [HttpGet("latest-otp/{nationalNumber}")]
        public async Task<IActionResult> GetLatestOtp(string nationalNumber, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetUserIdByNationalNumberAsync(nationalNumber, cancellationToken);
            if (!userId.HasValue) return NotFound("User not found.");
            var code = await _context.VerificationCodes
                .Where(vc => vc.UserId == userId.Value && !vc.IsUsed)
                .OrderByDescending(vc => vc.CreatedAt)
                .Select(vc => vc.Code)
                .FirstOrDefaultAsync(cancellationToken);
            return Ok(new { NationalNumber = nationalNumber, LatestOTP = code });
        }

        [HttpGet("db-check")]
        public async Task<IActionResult> CheckDatabaseConnection()
        {
            bool canConnect = await _context.Database.CanConnectAsync();

            if (!canConnect)
            {
                return StatusCode(500, new { Status = "Error", Message = "Failed to connect to SQL Server database." });
            }

            int personsCount = await _context.Persons.CountAsync();
            int usersCount = await _context.Users.CountAsync();

            return Ok(new
            {
                Status = "Success",
                Message = "Successfully connected to SQL Server via ApplicationDbContext!",
                CanConnect = canConnect,
                DatabaseName = _context.Database.GetDbConnection().Database,
                PersonsCount = personsCount,
                UsersCount = usersCount
            });
        }
    }

    
}
