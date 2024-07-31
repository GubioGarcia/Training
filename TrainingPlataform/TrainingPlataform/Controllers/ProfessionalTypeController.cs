
using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;

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
    }
}
