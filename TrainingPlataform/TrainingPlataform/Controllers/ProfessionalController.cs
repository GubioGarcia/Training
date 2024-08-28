using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            return Ok(this.professionalService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(this.professionalService.GetByid(id));
        }

        [HttpGet("ProfessionalByCpf/{cpf}")]
        public IActionResult GetByCpf(string cpf)
        {
            return Ok(this.professionalService.GetByCpf(cpf));
        }

        [HttpPost]
        public IActionResult Post(ProfessionalRequestViewModel professionalRequestViewModel)
        {
            return Ok(this.professionalService.Post(professionalRequestViewModel));
        }

        [HttpPut]
        public IActionResult Put(ProfessionalRequestUpdateViewModel professionalRequestUpdateViewModel)
        {
            return Ok(this.professionalService.Put(professionalRequestUpdateViewModel));
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            string _professionalId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);
            
            return Ok(this.professionalService.Delete(_professionalId));
        }

        [HttpPost("authenticate"), AllowAnonymous]
        public IActionResult Authenticate(UserAuthenticateRequestViewModel professionalViewModel)
        {
            return Ok(this.professionalService.Authenticate(professionalViewModel));
        }
    }
}
