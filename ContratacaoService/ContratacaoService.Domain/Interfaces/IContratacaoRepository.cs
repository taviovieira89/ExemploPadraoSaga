using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Domain.Interfaces
{
    public interface IContratacaoRepository
    {
        Task AddAsync(Contratacao contratacao);
        Task<Contratacao> GetByIdAsync(Guid id);
        Task<IEnumerable<Contratacao>> GetAllAsync();
    }
}