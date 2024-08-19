using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;
using Training.Application.ViewModels;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly IUserTypeService userTypeService;

        public UserTypeController(IUserTypeService userTypeService)
        {
            this.userTypeService = userTypeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.userTypeService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(this.userTypeService.GetById(id));
        }

        [HttpPost]
        public IActionResult Post(UserTypeViewModel userTypeViewModel)
        {
            return Ok(this.userTypeService.Post(userTypeViewModel));
        }

        [HttpPut]
        public IActionResult Put(UserTypeViewModel userTypeViewModel)
        {
            return Ok(this.userTypeService.Put(userTypeViewModel));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(this.userTypeService.Delete(id));
        }
    }
}
