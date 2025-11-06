using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;

namespace PropostaService.Domain.Interfaces
{
    public interface IPropostaRepository
    {
        Task AddAsync(Proposta proposta);
        Task<Proposta> GetByIdAsync(Guid id);
        Task<IEnumerable<Proposta>> GetAllAsync();
        Task UpdateAsync(Proposta proposta);
    }
}