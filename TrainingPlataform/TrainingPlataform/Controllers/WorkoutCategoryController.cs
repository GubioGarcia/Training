using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Application.ViewModels.ClientProfessionalViewModels;
using Training.Application.ViewModels.WorkoutCategoryViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class WorkoutCategoryController : ControllerBase
    {
        private readonly IWorkoutCategoryService workoutCategoryService;

        public WorkoutCategoryController(IWorkoutCategoryService workoutCategoryService)
        {
            this.workoutCategoryService = workoutCategoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.Get(_tokenId));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.GetById(id, _tokenId));
        }

        [HttpGet("WorkoutCategoryByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.GetByName(name, _tokenId));
        }

        [HttpGet("WorkoutCategoryByProfessional/{id}")]
        public IActionResult GetByProfessional(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.GetByProfessional(id, _tokenId));
        }

        [HttpPost]
        public IActionResult Post(WorkoutCategoryRequestViewModel workoutCategoryRequestViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.Post(_tokenId, workoutCategoryRequestViewModel));
        }
    }
}
