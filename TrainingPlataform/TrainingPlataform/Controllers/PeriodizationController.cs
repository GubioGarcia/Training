using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class PeriodizationController : ControllerBase
    {
        private readonly IPeriodizationService periodizationService;

        public PeriodizationController(IPeriodizationService periodizationService)
        {
            this.periodizationService = periodizationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.periodizationService.Get(_tokenId));
        }
    }
}
