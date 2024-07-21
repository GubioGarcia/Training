using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training.Application.Interfaces;

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
    }
}
