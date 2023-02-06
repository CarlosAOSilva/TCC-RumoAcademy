using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ApiTarefas.Services;
using ApiTarefas.Domain.Models;

namespace ApiSistemaRegistro.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class AutorizationController : ControllerBase
    {
        private readonly AutorizationService _service;
        public AutorizationController(AutorizationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Metodo para autenticar no sistema -
        /// Campos obrigatórios: email e senha
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Retorna um bearer token de 12 horas com nivel de acesso e nome do usuário</returns>
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(401)]
        [HttpPost("autorizacao")]
        public IActionResult Login(Usuario model)
        {
            try
            {
                var token = _service.Login(model);
                return StatusCode(200, token);
            }
            catch (Exception)
            {
                return StatusCode(401);
            }
        }
    }
}



