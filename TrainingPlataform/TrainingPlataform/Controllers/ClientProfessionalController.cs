using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.ViewModels.ClientProfessionalViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/ClientProfessionalRelationship")]
    [ApiController, Authorize]
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
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientProfessionalService.Get(_tokenId));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientProfessionalService.GetById(_tokenId, id));
        }

        [HttpGet("RelatedClientByProfessional/{id}")]
        public IActionResult GetClientsByProfessionalId(string id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientProfessionalService.GetClientsByProfessionalId(_tokenId, id));
        }

        [HttpPost]
        public IActionResult Post(ClientProfessionalRequestViewModel clientProfessionalRequestViewModels)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientProfessionalService.Post(_tokenId, clientProfessionalRequestViewModels));
        }

        [HttpPut]
        public IActionResult Put(ClientProfessionalRequestUpdateViewModel clientProfessionalRequestUpdateViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientProfessionalService.Put(_tokenId, clientProfessionalRequestUpdateViewModel));
        }

        [HttpDelete]
        public IActionResult Delete(string professionalId, string clientId)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientProfessionalService.Delete(_tokenId, professionalId, clientId));
        }
    }
}
