using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;
using Training.Application.ViewModels.ClientViewModels;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.clientService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(this.clientService.GetById(id));
        }

        [HttpPost]
        public IActionResult Post(ClientRequestViewModel clientRequestView)
        {
            return Ok(this.clientService.Post(clientRequestView));
        }

        [HttpPut]
        public IActionResult Put(ClientRequestViewModel clientRequestView)
        {
            return Ok(this.clientService.Put(clientRequestView));
        }
    }
}
