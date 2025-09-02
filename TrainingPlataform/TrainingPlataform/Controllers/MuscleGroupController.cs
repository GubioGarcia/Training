using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.ViewModels.MuscleGroupViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class MuscleGroupController : ControllerBase
    {
        private readonly IMuscleGroupService muscleGroupService;

        public MuscleGroupController(IMuscleGroupService muscleGroupService)
        {
            this.muscleGroupService = muscleGroupService;
        }

        /// <summary>
        /// Retorna os grupos musculares disponíveis para o usuário logado.
        /// </summary>
        /// <returns>Lista dos grupos musculares.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.Get(_tokenId));
        }

        /// <summary>
        /// Retorna um grupo muscular específico pelo ID.
        /// </summary>
        /// <param name="id">Identificador do grupo muscular.</param>
        /// <returns>Objeto MuscleGroupViewModel correspondente.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.GetById(id, _tokenId));
        }

        /// <summary>
        /// Retorna um grupo muscular com base no nome informado.
        /// </summary>
        /// <param name="name">Nome do grupo muscular.</param>
        /// <returns>Objeto MuscleGroupViewModel correspondente.</returns>
        [HttpGet("MuscleGroupByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.GetByName(name, _tokenId));
        }

        /// <summary>
        /// Retorna os grupos musculares associados a um profissional específico.
        /// </summary>
        /// <param name="id">Identificador do profissional.</param>
        /// <returns>Lista de grupos musculares vinculados ao profissional.</returns>
        [HttpGet("MuscleGroupByProfessional/{id}")]
        public IActionResult GetByProfessional(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.GetByProfessional(id, _tokenId));
        }

        /// <summary>
        /// Cria um grupo muscular.
        /// </summary>
        /// <param name="_muscleGroupRequestViewModel">Dados do grupo muscular a ser criado.</param>
        /// <returns>Objeto MuscleGroupRequestViewModel criado.</returns>
        [HttpPost]
        public IActionResult Post(MuscleGroupRequestViewModel _muscleGroupRequestViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.Post(_tokenId, _muscleGroupRequestViewModel));
        }

        /// <summary>
        /// Atualiza um grupo muscular existente.
        /// </summary>
        /// <param name="_muscleGroupUpdateViewModel">Dados atualizados do grupo muscular.</param>
        /// <returns>Objeto MuscleGroupUpdateViewModel atualizado.</returns>
        [HttpPut]
        public IActionResult Put(MuscleGroupUpdateViewModel _muscleGroupUpdateViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.muscleGroupService.Put(_tokenId, _muscleGroupUpdateViewModel));
        }

        /// <summary>
        /// Exclui um grupo muscular com base no ID informado.
        /// </summary>
        /// <param name="id">Identificador do grupo muscular a ser excluído.</param>
        /// <returns>Resposta HTTP indicando sucesso da operação.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            this.muscleGroupService.Delete(_tokenId, id);
            return Ok();
        }
    }
}
