using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;

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
    }
}
