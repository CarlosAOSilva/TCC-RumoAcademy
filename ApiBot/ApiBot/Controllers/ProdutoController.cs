using ApiBot.Domain.Models;
using ApiBot.Service;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ApiBot.Controllers
{
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;
        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        /// <summary>
        /// - Gerar uma Lista de produtos Coletados pelo robô -
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso!!!</response>
        [HttpGet("produto")]
        public IActionResult Listar([FromQuery] string? nomeProduto)
        {
            return StatusCode(200, _service.Listar(nomeProduto));
        }

        /// <summary>
        /// - Acionar o Robô para coletar Produtos do site selecionado - 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso!!!</response>
        [HttpPost("produto")]
        public IActionResult Inserir()
        {
            try
            {
                _service.Inserir();
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
