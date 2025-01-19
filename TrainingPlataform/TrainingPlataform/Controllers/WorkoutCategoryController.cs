using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Services;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class WorkoutCategoryController : Controller
    {
        private readonly WorkoutCategoryService workoutCategoryService;

        public WorkoutCategoryController(WorkoutCategoryService workoutCategoryService)
        {
            this.workoutCategoryService = workoutCategoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.Get(_tokenId));
        }
    }
}
