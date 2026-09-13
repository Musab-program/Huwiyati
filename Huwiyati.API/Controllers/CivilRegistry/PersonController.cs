using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.API.Controllers.CivilRegistry
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        public PersonController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        //[Authorize(Roles = $"{AppRoles.Employee},{AppRoles.Admin},{AppRoles.SuperAdmin}")]
        [Authorize(Roles =AppRoles.Citizen)]
        public async Task<IActionResult> GetAllCitizens(CancellationToken cancellation)
        {
            var persons = await _context.Persons.ToListAsync(cancellation);
            return Ok(persons);
        }
    }
}
