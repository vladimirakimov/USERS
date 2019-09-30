using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Fabric;

namespace ITG.Brix.Users.API.Controllers
{
    [Route("/")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class HealthController : ControllerBase
    {
        private readonly StatelessServiceContext _context;

        public HealthController(StatelessServiceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult CheckHealth()
        {
            return Ok(new
            {
                service = _context.ServiceName,
                version = _context.CodePackageActivationContext.CodePackageVersion,
                status = "Alive and kicking"
            });
        }
    }
}
