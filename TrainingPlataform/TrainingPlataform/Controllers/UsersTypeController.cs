using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;
using Training.Application.ViewModels;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersTypeController : ControllerBase
    {
        private readonly IUserTypeService usersTypeService;

        public UsersTypeController(IUserTypeService usersTypeService)
        {
            this.usersTypeService = usersTypeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.usersTypeService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(this.usersTypeService.GetById(id));
        }

        [HttpPost]
        public IActionResult Post(UsersTypeViewModel usersTypeViewModel)
        {
            return Ok(this.usersTypeService.Post(usersTypeViewModel));
        }

        [HttpPut]
        public IActionResult Put(UsersTypeViewModel usersTypeViewModel)
        {
            return Ok(this.usersTypeService.Put(usersTypeViewModel));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(this.usersTypeService.Delete(id));
        }
    }
}
