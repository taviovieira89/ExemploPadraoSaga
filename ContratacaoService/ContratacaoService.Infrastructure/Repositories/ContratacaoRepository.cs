using Microsoft.EntityFrameworkCore;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Interfaces;
using ContratacaoService.Infrastructure.Data;

namespace ContratacaoService.Infrastructure.Repositories
{
    public class ContratacaoRepository : IContratacaoRepository
    {
        private readonly ContratacaoDbContext _context;

        public ContratacaoRepository(ContratacaoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contratacao contratacao)
        {
            await _context.Contratacoes.AddAsync(contratacao);
            await _context.SaveChangesAsync();
        }

        public async Task<Contratacao> GetByIdAsync(Guid id)
        {
            return await _context.Contratacoes.FindAsync(id);
        }

        public async Task<IEnumerable<Contratacao>> GetAllAsync()
        {
            return await _context.Contratacoes.ToListAsync();
        }
    }
}