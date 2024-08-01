using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;
using Training.Application.ViewModels;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalTypeController : Controller
    {
        private readonly IProfessionalTypeService professionalTypeService;

        public ProfessionalTypeController(IProfessionalTypeService professionalTypeService)
        {
            this.professionalTypeService = professionalTypeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.professionalTypeService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {

            return Ok(this.professionalTypeService.GetById(id));
        }

        [HttpPost]
        public IActionResult Post(ProfessionalTypeViewModel professionalTypeViewModel)
        {
            return Ok(this.professionalTypeService.Post(professionalTypeViewModel));
        }

        
    }
}
