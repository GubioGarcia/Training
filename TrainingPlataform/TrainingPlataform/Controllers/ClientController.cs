using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.ViewModels.AuthenticateViewModels;
using Training.Application.ViewModels.ClientViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.Get(_tokenId));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.GetById(id, _tokenId));
        }

        [HttpGet("ClientByCPF/{cpf:length(11)}")]
        public IActionResult GetByCpf(string cpf)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.GetByCpf(cpf, _tokenId));
        }

        [HttpGet("ClientByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.GetByName(name, _tokenId));
        }

        [HttpPost]
        public IActionResult Post(ClientRequestViewModel clientRequestView)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.Post(clientRequestView, _tokenId));
        }

        [HttpPut]
        public IActionResult Put(ClientRequestUpdateViewModel clientRequestUpdateViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.Put(clientRequestUpdateViewModel, _tokenId));
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            string _clientId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.Delete(_clientId));
        }

        [HttpPost("authenticate"), AllowAnonymous]
        public IActionResult Authenticate(UserAuthenticateRequestViewModel clientViewModel)
        {
            return Ok(this.clientService.Authenticate(clientViewModel));
        }
    }
}
