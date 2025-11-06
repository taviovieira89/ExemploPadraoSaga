using PropostaService.Application.DTOs;
using PropostaService.Domain.Enums;

namespace PropostaService.Application.Interfaces
{
    public interface IPropostaAppService
    {
        Task<PropostaDTO> CriarPropostaAsync(CriarPropostaDTO criarPropostaDTO);
        Task<PropostaDTO> GetPropostaByIdAsync(Guid id);
        Task<IEnumerable<PropostaDTO>> GetAllPropostasAsync();
        Task UpdatePropostaStatusAsync(Guid id, StatusProposta status);
    }
}