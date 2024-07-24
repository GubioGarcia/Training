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
        private readonly IUsersTypeService usersTypeService;

        public UsersTypeController(IUsersTypeService usersTypeService)
        {
            this.usersTypeService = usersTypeService;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            return Ok(this.usersTypeService.Get());
        }

        [HttpPost]
        public IActionResult Post(UsersTypeViewModel usersTypeViewModel)
        {
            return Ok(this.usersTypeService.Post(usersTypeViewModel));
        }
    }
}
