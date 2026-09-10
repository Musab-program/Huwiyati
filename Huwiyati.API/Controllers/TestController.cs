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

        public TestController(ApplicationDbContext context)
        {
            _context = context;
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
