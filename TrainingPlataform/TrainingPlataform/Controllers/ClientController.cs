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

        /// <summary>
        /// Obtém a lista de todos os clientes associados ao usuário autenticado.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.Get(_tokenId));
        }

        /// <summary>
        /// Obtém os dados de um cliente específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.GetById(id, _tokenId));
        }

        /// <summary>
        /// Obtém os dados de um cliente a partir do CPF informado.
        /// </summary>
        /// <param name="cpf">CPF do cliente (11 dígitos).</param>
        [HttpGet("ClientByCPF/{cpf:length(11)}")]
        public IActionResult GetByCpf(string cpf)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.GetByCpf(cpf, _tokenId));
        }

        /// <summary>
        /// Obtém a lista de clientes com nome semelhante ao informado.
        /// </summary>
        /// <param name="name">Nome ou parte do nome do cliente.</param>
        [HttpGet("ClientByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.GetByName(name, _tokenId));
        }

        /// <summary>
        /// Cadastra um novo cliente no sistema.
        /// </summary>
        /// <param name="clientRequestView">Dados do cliente a ser criado.</param>
        [HttpPost]
        public IActionResult Post(ClientRequestViewModel clientRequestView)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.Post(clientRequestView, _tokenId));
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="clientRequestUpdateViewModel">Dados atualizados do cliente.</param>
        [HttpPut]
        public IActionResult Put(ClientRequestUpdateViewModel clientRequestUpdateViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.Put(clientRequestUpdateViewModel, _tokenId));
        }

        /// <summary>
        /// Exclui logicamente o cliente autenticado do sistema.
        /// </summary>
        [HttpDelete]
        public IActionResult Delete()
        {
            string _clientId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.clientService.Delete(_clientId));
        }

        /// <summary>
        /// Realiza a autenticação do cliente no sistema.
        /// </summary>
        /// <param name="clientViewModel">Dados de login do cliente (CPF e senha).</param>
        [HttpPost("authenticate"), AllowAnonymous]
        public IActionResult Authenticate(UserAuthenticateRequestViewModel clientViewModel)
        {
            return Ok(this.clientService.Authenticate(clientViewModel));
        }
    }
}
