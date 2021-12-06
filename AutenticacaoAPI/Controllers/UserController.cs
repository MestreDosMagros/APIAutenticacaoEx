using AutenticacaoAPI.DTOs;
using AutenticacaoAPI.Models;
using AutenticacaoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AutenticacaoAPI.Controllers
{
    [ApiController, Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO userDTO)
        {
            var user = new User
            {
                Role = userDTO.Role,
                Username = userDTO.Username,
                Password = userDTO.Password
            };

            return Ok(_userService.Create(user));
        }

        [HttpPost, Route("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO user)
        {
            return Ok(_userService.Login(user.Username, user.Password));
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";
    }
}
