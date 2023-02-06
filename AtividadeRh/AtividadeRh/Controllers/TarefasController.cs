using ApiTarefas.Domain.Models;
using ApiTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiSistemaRegistro.Controllers
{
    [Authorize]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly TarefasService _service;
        private readonly RelatorioService _relatorioService;
        public TarefasController(TarefasService service, RelatorioService relatorioService)
        {
            _service = service;
            _relatorioService = relatorioService;
        }

        /// <summary>
        /// - Rota que permite listar tarefas registradas - 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna os elementos encontrados via ID</response>
        [Authorize(Roles = "1,2")]
        [HttpGet("tarefa")]
        public IActionResult Listar()
        {
            return StatusCode(200, _service.ListarTarefas());
        }

        /// <summary>
        /// - Rota que permite listar uma tarefa em específico -
        /// Campos obrigatórios: tarefaId -
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1,2")]
        [HttpGet("tarefa/{tarefaId}")]
        public IActionResult Obter([FromRoute] int tarefaId)
        {
            return StatusCode(200, _service.Obter(tarefaId));
        }

        /// <summary>
        /// - Rota para realizar registros de tarefas - 
        /// Campos obrigatórios: tarefaId, horarioInicio, horarioFim, resumoTarefa, descricaoTarefa, tipoTarefaId -
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1")]
        [HttpPost("tarefa")]
        public IActionResult Inserir([FromBody] Tarefas model)
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
        /// - Rota que permite apagar o registro de uma tarefa em específico -
        /// - Campos obrigatórios: tarefaId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1")]
        [HttpDelete("tarefa/{tarefaId}")]
        public IActionResult Deletar([FromRoute] int tarefaId)
        {
            _service.Deletar(tarefaId);
            return StatusCode(200);
        }


        /// <summary>
        /// - Rota que permite atualizar o registro de uma tarefa em específico -
        /// Campos obrigatórios: - tarefaId, horarioInicio, horarioFim, resumoTarefa, descricaoTarefa, tipoTarefaId -
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1")]
        [HttpPut("tarefa")]
        public IActionResult Atualizar([FromBody] Tarefas model)
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
        [Authorize(Roles = "2")]
        [HttpGet("relatorio")]
        public IActionResult Relatorio()
        {
            return File(_relatorioService.GerarRelatorioTarefas(), contentType: "text/csv", fileDownloadName: "relatorio.csv", enableRangeProcessing: true);
        }
    }
}





