using Microsoft.AspNetCore.Mvc;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Interfaces;

namespace ContratacaoService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratacoesController : ControllerBase
    {
        private readonly IContratacaoAppService _contratacaoAppService;

        public ContratacoesController(IContratacaoAppService contratacaoAppService)
        {
            _contratacaoAppService = contratacaoAppService;
        }

        [HttpPost]
        public async Task<IActionResult> ContratarProposta([FromBody] ContratarPropostaDTO dto)
        {
            try
            {
                var contratacao = await _contratacaoAppService.ContratarPropostaAsync(dto);
                return CreatedAtAction(nameof(GetContratacaoById), new { id = contratacao.Id }, contratacao);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContratacaoById(Guid id)
        {
            var contratacao = await _contratacaoAppService.GetContratacaoByIdAsync(id);
            if (contratacao == null)
            {
                return NotFound();
            }
            return Ok(contratacao);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContratacoes()
        {
            var contratacoes = await _contratacaoAppService.GetAllContratacoesAsync();
            return Ok(contratacoes);
        }
    }
}