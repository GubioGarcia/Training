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
            return Ok(this.clientService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(this.clientService.GetById(id));
        }

        [HttpPost]
        public IActionResult Post(ClientRequestViewModel clientRequestView)
        {
            return Ok(this.clientService.Post(clientRequestView));
        }

        [HttpPut]
        public IActionResult Put(ClientRequestUpdateViewModel clientRequestUpdateViewModel)
        {
            return Ok(this.clientService.Put(clientRequestUpdateViewModel));
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
