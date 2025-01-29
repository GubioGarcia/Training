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
    public class PeriodizationTrainingController : ControllerBase
    {
        private readonly IPeriodizationTrainingService periodizationTrainingService;

        public PeriodizationTrainingController(IPeriodizationTrainingService periodizationTrainingService)
        {
            this.periodizationTrainingService = periodizationTrainingService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.periodizationTrainingService.Get(_tokenId));
        }
    }
}
