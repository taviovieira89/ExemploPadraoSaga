using ContratacaoService.Application.DTOs; // Para PropostaDTO, se necessário
using System.Threading.Tasks;

namespace ContratacaoService.Application.Interfaces
{
    public interface IPropostaServiceHttpClient
    {
        Task<PropostaStatusDTO> GetPropostaStatusAsync(Guid propostaId);
        Task UpdatePropostaStatusAsync(Guid propostaId, string status);
    }

    public class PropostaStatusDTO
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}