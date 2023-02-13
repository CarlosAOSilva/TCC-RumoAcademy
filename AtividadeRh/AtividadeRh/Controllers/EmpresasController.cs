
using ApiTarefas.Domain.Models;
using ApiTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiSistemaRegistro.Controllers
{
    //[Authorize]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly EmpresasService _service;
        public EmpresasController(EmpresasService service)
        {
            _service = service;
        }

        /// <summary>
        /// - Rota que permite listar as empresas registradas - Você pode buscar uma empresa em especifico -
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna os elementos encontrados via CNPJ</response>
        //[Authorize(Roles = "1")]
        [HttpGet("empresas")]
        public IActionResult Listar([FromQuery] string? cnpj)
        {
            return StatusCode(200, _service.ListarEmpresas(cnpj));
        }

        /// <summary>
        /// - Rota para realizar O cadastro de uma Empresa - 
        /// - Campos obrigatórios: CNPJ, razaoSocial, dataCadastro -
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna os elementos cadastrados</response>
        //[Authorize(Roles = "1")]
        [HttpPost("empresas")]
        public IActionResult Inserir([FromBody] Empresas model)
        {
            try
            {
                _service.Inserir(model);
                return StatusCode(201);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        /// <summary>
        /// - Rota para apagar um registro de uma Empresa em específico - 
        /// - Campos obrigatórios: CNPJ -
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via CNPJ</response>
        //[Authorize(Roles = "1")]
        [HttpDelete("empresas/{cnpj}")]
        public IActionResult Deletar([FromRoute] string? cnpj)
        {
            _service.Deletar(cnpj);
            return StatusCode(200);
        }

        /// <summary>
        /// - Rota para atualizar dados de uma Empresa - 
        /// - Campos obrigatórios: CNPJ, razaoSocial, dataCadastro -
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via CNPJ </response>
        //[Authorize(Roles = "1")]
        [HttpPut("empresas")]
        public IActionResult Atualizar([FromBody] Empresas model)
        {
            try
            {
                _service.Atualizar(model);
                return StatusCode(201);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
