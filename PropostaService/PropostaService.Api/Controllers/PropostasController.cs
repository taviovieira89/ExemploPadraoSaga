using Microsoft.AspNetCore.Mvc;
using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;
using PropostaService.Domain.Enums;

namespace PropostaService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropostasController : ControllerBase
    {
        private readonly IPropostaAppService _propostaAppService;

        public PropostasController(IPropostaAppService propostaAppService)
        {
            _propostaAppService = propostaAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarProposta([FromBody] CriarPropostaDTO dto)
        {
            var proposta = await _propostaAppService.CriarPropostaAsync(dto);
            return CreatedAtAction(nameof(GetPropostaById), new { id = proposta.Id }, proposta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPropostaById(Guid id)
        {
            var proposta = await _propostaAppService.GetPropostaByIdAsync(id);
            if (proposta == null)
            {
                return NotFound();
            }
            return Ok(proposta);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPropostas()
        {
            var propostas = await _propostaAppService.GetAllPropostasAsync();
            return Ok(propostas);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdatePropostaStatus(Guid id, [FromQuery] StatusProposta status)
        {
            try
            {
                await _propostaAppService.UpdatePropostaStatusAsync(id, status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}