using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Training.Application.Interfaces;
using Training.Application.ViewModels;
using Training.Auth.Services;

namespace TrainingPlataform.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
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
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.usersTypeService.Get(_tokenId));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.usersTypeService.GetById(_tokenId, id));
        }

        [HttpPost]
        public IActionResult Post(UsersTypeViewModel usersTypeViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.usersTypeService.Post(_tokenId, usersTypeViewModel));
        }

        [HttpPut]
        public IActionResult Put(UsersTypeViewModel usersTypeViewModel)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.usersTypeService.Put(_tokenId, usersTypeViewModel));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            string _tokenId = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.NameIdentifier);

            return Ok(this.usersTypeService.Delete(_tokenId, id));
        }
    }
}
