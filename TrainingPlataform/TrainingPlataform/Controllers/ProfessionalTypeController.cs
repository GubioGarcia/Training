using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.ViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ProfessionalTypeController : ControllerBase
    {
        private readonly IProfessionalTypeService professionalTypeService;

        public ProfessionalTypeController(IProfessionalTypeService professionalTypeService)
        {
            this.professionalTypeService = professionalTypeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalTypeService.Get(_tokenId));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalTypeService.GetById(_tokenId, id));
        }

        [HttpPost]
        public IActionResult Post(ProfessionalTypeViewModel professionalTypeViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalTypeService.Post(_tokenId, professionalTypeViewModel));
        }

        [HttpPut]
        public IActionResult Put(ProfessionalTypeViewModel professionalTypeViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalTypeService.Put(_tokenId, professionalTypeViewModel));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalTypeService.Delete(_tokenId, id));
        }
    }
}
