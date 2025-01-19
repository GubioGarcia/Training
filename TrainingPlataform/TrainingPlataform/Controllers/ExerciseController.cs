using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Services;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ExerciseController : Controller
    {
        private readonly ExerciseService exerciseService;

        public ExerciseController(ExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.exerciseService.Get(_tokenId));
        }
    }
}
