using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.ViewModels.MuscleGroupViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class MuscleGroupController : ControllerBase
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

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.GetById(id, _tokenId));
        }

        [HttpGet("MuscleGroupByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.GetByName(name, _tokenId));
        }

        [HttpGet("MuscleGroupByProfessional/{id}")]
        public IActionResult GetByProfessional(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.GetByProfessional(id, _tokenId));
        }

        [HttpPost]
        public IActionResult Post(MuscleGroupRequestViewModel _muscleGroupRequestViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.Post(_tokenId, _muscleGroupRequestViewModel));
        }
    }
}
