using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class MuscleGroupController : Controller
    {
        private readonly IMuscleGroupService muscleGroupService;

        public MuscleGroupController(IMuscleGroupService muscleGroupService)
        {
            this.muscleGroupService = muscleGroupService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.Get(_tokenId));
        }
    }
}
