using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Application.ViewModels.ClientProfessionalViewModels;
using Training.Application.ViewModels.MuscleGroupViewModels;
using Training.Application.ViewModels.WorkoutCategoryViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class WorkoutCategoryController : ControllerBase
    {
        private readonly IWorkoutCategoryService workoutCategoryService;

        public WorkoutCategoryController(IWorkoutCategoryService workoutCategoryService)
        {
            this.workoutCategoryService = workoutCategoryService;
        }


        /// <summary>
        /// Retorna as categorias de treino disponíveis para o usuário logado.
        /// </summary>
        /// <returns>Lista de categorias de treino.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.Get(_tokenId));
        }

        /// <summary>
        /// Retorna uma categoria de treino específica pelo ID.
        /// </summary>
        /// <param name="id">Identificador da categoria de treino.</param>
        /// <returns>Objeto WorkoutCategoryViewModel correspondente.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.GetById(id, _tokenId));
        }

        /// <summary>
        /// Retorna uma categoria de treino com base no nome informado.
        /// </summary>
        /// <param name="name">Nome da categoria de treino.</param>
        /// <returns>Objeto WorkoutCategoryViewModel correspondente.</returns>
        [HttpGet("WorkoutCategoryByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.GetByName(name, _tokenId));
        }

        /// <summary>
        /// Retorna as categorias de treino associadas a um profissional específico.
        /// </summary>
        /// <param name="id">Identificador do profissional.</param>
        /// <returns>Lista de categorias de treino vinculadas ao profissional.</returns>
        [HttpGet("WorkoutCategoryByProfessional/{id}")]
        public IActionResult GetByProfessional(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.GetByProfessional(id, _tokenId));
        }

        /// <summary>
        /// Cria uma nova categoria de treino.
        /// </summary>
        /// <param name="_workoutCategoryRequestViewModel">Dados da categoria de treino a ser criada.</param>
        /// <returns>Objeto WorkoutCategoryViewModel criado.</returns>
        [HttpPost]
        public IActionResult Post(WorkoutCategoryRequestViewModel _workoutCategoryRequestViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.Post(_tokenId, _workoutCategoryRequestViewModel));
        }

        /// <summary>
        /// Atualiza uma categoria de treino existente.
        /// </summary>
        /// <param name="_workoutCategoryUpdateViewModel">Dados atualizados da categoria de treino.</param>
        /// <returns>Objeto WorkoutCategoryViewModel atualizado.</returns>
        [HttpPut]
        public IActionResult Put(WorkoutCategoryUpdateViewModel _workoutCategoryUpdateViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.workoutCategoryService.Put(_tokenId, _workoutCategoryUpdateViewModel));
        }

        /// <summary>
        /// Exclui uma categoria de treino com base no ID informado.
        /// </summary>
        /// <param name="id">Identificador da categoria de treino a ser excluída.</param>
        /// <returns>Resposta HTTP indicando sucesso da operação.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            this.workoutCategoryService.Delete(_tokenId, id);
            return Ok();
        }
    }
}
