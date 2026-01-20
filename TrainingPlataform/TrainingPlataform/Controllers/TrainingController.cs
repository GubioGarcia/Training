using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService trainingService;

        public TrainingController(ITrainingService trainingService)
        {
            this.trainingService = trainingService;
        }

        /// <summary>
        /// Obtém a lista de todos os treinos, buscando treinos base do sistema e treinos personalizados do profissional logado.
        /// </summary>
        /// <returns>Lista de treinos disponíveis para o usuário logado.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.trainingService.Get(_tokenId));
        }

        /// <summary>
        /// Retorna um treino específico pelo ID.
        /// </summary>
        /// <param name="id">Identificador do treino.</param>
        /// <returns>Objeto TrainingViewModel correspondente.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.trainingService.GetById(id, _tokenId));
        }

        /// <summary>
        /// Retorna um treino com base no nome informado.
        /// </summary>
        /// <param name="name">Nome do treino.</param>
        /// <returns>Objeto TrainingViewModel correspondente.</returns>
        [HttpGet("TrainingByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.trainingService.GetByName(name, _tokenId));
        }

        /// <summary>
        /// Retorna os treinos associados a um profissional específico.
        /// </summary>
        /// <param name="id">Identificador do profissional.</param>
        /// <returns>Lista de treinos vinculados ao profissional.</returns>
        [HttpGet("TrainingByProfessional/{id}")]
        public IActionResult GetByProfessional(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.trainingService.GetByProfessional(id, _tokenId));
        }
    }
}
