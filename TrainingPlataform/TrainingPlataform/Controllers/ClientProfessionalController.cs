using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.ViewModels.ClientProfessionalViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/ClientProfessionalRelationship")]
    [ApiController]
    public class ClientProfessionalController : ControllerBase
    {
        private readonly IClientProfessionalService clientProfessionalService;

        public ClientProfessionalController(IClientProfessionalService clientProfessionalService)
        {
            this.clientProfessionalService = clientProfessionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.clientProfessionalService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(this.clientProfessionalService.GetById(id));
        }

        [HttpGet("RelatedClientByProfessional/{id}")]
        public IActionResult GetClientsByProfessionalId(string id)
        {
            return Ok(this.clientProfessionalService.GetClientsByProfessionalId(id));
        }

        [HttpPost]
        public IActionResult Post(ClientProfessionalRequestViewModel clientProfessionalRequestViewModels)
        {
            return Ok(this.clientProfessionalService.Post(clientProfessionalRequestViewModels));
        }

        [HttpPut]
        public IActionResult Put(ClientProfessionalRequestUpdateViewModel clientProfessionalRequestUpdateViewModel)
        {
            return Ok(this.clientProfessionalService.Put(clientProfessionalRequestUpdateViewModel));
        }

        [HttpDelete]
        public IActionResult Delete(string professionalId, string clientId)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientProfessionalService.Delete(_tokenId, professionalId, clientId));
        }
    }
}
