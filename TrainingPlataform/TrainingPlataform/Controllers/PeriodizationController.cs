using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Application.ViewModels.ExerciseViewModels;
using Training.Application.ViewModels.PeriodizationViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class PeriodizationController : ControllerBase
    {
        private readonly IPeriodizationService periodizationService;

        public PeriodizationController(IPeriodizationService periodizationService)
        {
            this.periodizationService = periodizationService;
        }

        /// <summary>
        /// Obtém a lista de todas as periodizações, buscando periodizações base do sistema e periodizações personalizadas do profissional logado.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.periodizationService.Get(_tokenId));
        }

        /// <summary>
        /// Retorna uma periodização específica pelo ID.
        /// </summary>
        /// <param name="id">Identificador da periodização.</param>
        /// <returns>Objeto PeriodizationViewModel correspondente.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.periodizationService.GetById(_tokenId, id));
        }

        /// <summary>
        /// Retorna as periodizações associados a um profissional específico.
        /// </summary>
        /// <param name="id">Identificador do profissional.</param>
        /// <returns>Lista de periodizações vinculados ao profissional.</returns>
        [HttpGet("PeriodizationByProfessional/{id}")]
        public IActionResult GetByProfessional(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.periodizationService.GetByProfessional(_tokenId, id));
        }

        /// <summary>
        /// Retorna as periodizações de um cliente.
        /// </summary>
        /// <param name="id">Identificador do cliente.</param>
        /// <returns>Lista de periodizações vinculados ao cliente.</returns>
        [HttpGet("PeriodizationByClient/{id}")]
        public IActionResult GetByClient(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.periodizationService.GetByProfessional(_tokenId, id));
        }

        /// <summary>
        /// Cria uma periodização de treino.
        /// </summary>
        /// <param name="_periodizationRequestViewModel">Dados da periodização a ser criada.</param>
        /// <returns>Objeto PeriodizationRequestViewModel criado.</returns>
        [HttpPost]
        public IActionResult Post(PeriodizationRequestViewModel _periodizationRequestViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.periodizationService.Post(_tokenId, _periodizationRequestViewModel));
        }

        /// <summary>
        /// Atualiza uma periodization existente.
        /// </summary>
        /// <param name="_periodizationUpdateViewModel">Dados atualizados da periodização.</param>
        /// <returns>Objeto PeriodizationUpdateViewModel atualizada.</returns>
        [HttpPut]
        public IActionResult Put(PeriodizationUpdateViewModel _periodizationUpdateViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.periodizationService.Put(_tokenId, _periodizationUpdateViewModel));
        }

        /// <summary>
        /// Exclui uma periodização com base no ID informado.
        /// </summary>
        /// <param name="id">Identificador da periodização a ser excluída.</param>
        /// <returns>Resposta HTTP indicando sucesso da operação.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            this.periodizationService.Delete(_tokenId, id);
            return Ok();
        }
    }
}
