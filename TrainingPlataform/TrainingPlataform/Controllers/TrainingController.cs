using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Services;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class TrainingController : Controller
    {
        private readonly TrainingService trainingService;

        public TrainingController(TrainingService trainingService)
        {
            this.trainingService = trainingService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.trainingService.Get(_tokenId));
        }
    }
}
