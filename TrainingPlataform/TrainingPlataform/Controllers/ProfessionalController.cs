using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Application.ViewModels.AuthenticateViewModels;
using Training.Application.ViewModels.ProfessionalViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ProfessionalController : ControllerBase
    {
        private readonly IProfessionalService professionalService;

        public ProfessionalController(IProfessionalService professionalService)
        {
            this.professionalService = professionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalService.Get(_tokenId));
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalService.GetByid(id, _tokenId));
        }

        [HttpGet("ProfessionalByCpf/{cpf:length(11)}")]
        public IActionResult GetByCpf(string cpf)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalService.GetByCpf(cpf, _tokenId));
        }

        [HttpGet("ProfessionalByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalService.GetByName(name, _tokenId));
        }

        [HttpPost]
        public IActionResult Post(ProfessionalRequestViewModel professionalRequestViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalService.Post(professionalRequestViewModel, _tokenId));
        }

        [HttpPut]
        public IActionResult Put(ProfessionalRequestUpdateViewModel professionalRequestUpdateViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.professionalService.Put(professionalRequestUpdateViewModel, _tokenId));
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);
            
            return Ok(this.professionalService.Delete(id, _tokenId));
        }

        [HttpPost("authenticate"), AllowAnonymous]
        public IActionResult Authenticate(UserAuthenticateRequestViewModel professionalViewModel)
        {
            return Ok(this.professionalService.Authenticate(professionalViewModel));
        }
    }
}
