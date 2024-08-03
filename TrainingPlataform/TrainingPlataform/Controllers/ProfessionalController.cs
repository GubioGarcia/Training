using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;
using Training.Application.ViewModels;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalController : ControllerBase
    {
        private readonly IProfessionalService professionalService;

        public ProfessionalController(IProfessionalService professionalService)
        {
            this.professionalService = professionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.professionalService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(this.professionalService.GetByid(id));
        }

        [HttpPost]
        public IActionResult Post(ProfessionalViewModel professionalViewModel)
        {
            return Ok(this.professionalService.Post(professionalViewModel));
        }
    }
}
