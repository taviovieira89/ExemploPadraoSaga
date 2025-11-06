using ContratacaoService.Application.DTOs;

namespace ContratacaoService.Application.Interfaces
{
    public interface IContratacaoAppService
    {
        Task<ContratacaoDTO> ContratarPropostaAsync(ContratarPropostaDTO contratarPropostaDTO);
        Task<ContratacaoDTO> GetContratacaoByIdAsync(Guid id);
        Task<IEnumerable<ContratacaoDTO>> GetAllContratacoesAsync();
    }
}