using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;

namespace TrainingPlataform.Controllers
{
    [Route("api/ClientProfessionalRelationship")]
    [ApiController]
    public class ClientProfessionalController : ControllerBase
    {
        private readonly IClientProfessionalService clientProfessionalService;

        public ClientProfessionalController(IClientProfessionalService clientProfessionalService)
        {
            this.clientProfessionalService = clientProfessionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.clientProfessionalService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(this.clientProfessionalService.GetById(id));
        }

        [HttpGet("RelatedClientByProfessional/{id}")]
        public IActionResult GetClientsByProfessionalId(string id)
        {
            return Ok(this.clientProfessionalService.GetClientsByProfessionalId(id));
        }
    }
}
