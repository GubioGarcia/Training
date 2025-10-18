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
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }

        /// <summary>
        /// Obtém a lista de todos os exercícios disponíveis, buscando exercícios base do sistema e exercícios personalizados do profissional logado.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.exerciseService.Get(_tokenId));
        }

        /// <summary>
        /// Retorna um exercício específico pelo ID.
        /// </summary>
        /// <param name="id">Identificador do exercício.</param>
        /// <returns>Objeto ExerciseViewModel correspondente.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.exerciseService.GetById(id, _tokenId));
        }

        /// <summary>
        /// Retorna uma lista exercícios com base no nome informado.
        /// </summary>
        /// <param name="name">Nome do exercício.</param>
        /// <returns>Objeto ExerciseViewModel correspondente.</returns>
        [HttpGet("ExerciseByName/{name:minlength(1)}")]
        public IActionResult GetByName(string name)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.exerciseService.GetByName(name, _tokenId));
        }

        /// <summary>
        /// Retorna os exercícios associados a um profissional específico.
        /// </summary>
        /// <param name="id">Identificador do profissional.</param>
        /// <returns>Lista de exercícios vinculados ao profissional.</returns>
        [HttpGet("ExerciseByProfessional/{id}")]
        public IActionResult GetByProfessional(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.exerciseService.GetByProfessional(id, _tokenId));
        }

        /// <summary>
        /// Retorna os exercícios associados a um grupo muscular específico.
        /// </summary>
        /// <param name="id">Identificador do grupo muscular.</param>
        /// <returns>Lista de exercícios vinculados ao grupo muscular.</returns>
        [HttpGet("ExerciseByMuscleGroup/{id}")]
        public IActionResult GetByMuscleGroup(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.exerciseService.GetByMuscleGroup(id, _tokenId));
        }

        /// <summary>
        /// Retorna os exercícios associados a uma categoria de trabalho específica.
        /// </summary>
        /// <param name="id">Identificador da categoria de trabalho.</param>
        /// <returns>Lista de exercícios vinculados a categoria de trabalho.</returns>
        [HttpGet("ExerciseByWorkoutCategory/{id}")]
        public IActionResult GetByWorkoutCategory(Guid id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.exerciseService.GetByWorkoutCategory(id, _tokenId));
        }
    }
}
