using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using PropostaService.Domain.Interfaces;

namespace PropostaService.Application.Services
{
    public class PropostaAppService : IPropostaAppService
    {
        private readonly IPropostaRepository _propostaRepository;

        public PropostaAppService(IPropostaRepository propostaRepository)
        {
            _propostaRepository = propostaRepository;
        }

        public async Task<PropostaDTO> CriarPropostaAsync(CriarPropostaDTO criarPropostaDTO)
        {
            var proposta = new Proposta(criarPropostaDTO.NomeCliente, criarPropostaDTO.CpfCliente, criarPropostaDTO.ValorSeguro);
            await _propostaRepository.AddAsync(proposta);
            return new PropostaDTO
            {
                Id = proposta.Id,
                NomeCliente = proposta.NomeCliente,
                CpfCliente = proposta.CpfCliente,
                ValorSeguro = proposta.ValorSeguro,
                Status = proposta.Status,
                DataCriacao = proposta.DataCriacao
            };
        }

        public async Task<PropostaDTO> GetPropostaByIdAsync(Guid id)
        {
            var proposta = await _propostaRepository.GetByIdAsync(id);
            if (proposta == null) return null;

            return new PropostaDTO
            {
                Id = proposta.Id,
                NomeCliente = proposta.NomeCliente,
                CpfCliente = proposta.CpfCliente,
                ValorSeguro = proposta.ValorSeguro,
                Status = proposta.Status,
                DataCriacao = proposta.DataCriacao
            };
        }

        public async Task<IEnumerable<PropostaDTO>> GetAllPropostasAsync()
        {
            var propostas = await _propostaRepository.GetAllAsync();
            return propostas.Select(p => new PropostaDTO
            {
                Id = p.Id,
                NomeCliente = p.NomeCliente,
                CpfCliente = p.CpfCliente,
                ValorSeguro = p.ValorSeguro,
                Status = p.Status,
                DataCriacao = p.DataCriacao
            });
        }

        public async Task UpdatePropostaStatusAsync(Guid id, StatusProposta status)
        {
            var proposta = await _propostaRepository.GetByIdAsync(id);
            if (proposta == null)
            {
                throw new KeyNotFoundException($"Proposta com ID {id} não encontrada.");
            }

            switch (status)
            {
                case StatusProposta.Aprovada:
                    proposta.Aprovar();
                    break;
                case StatusProposta.Rejeitada:
                    proposta.Rejeitar();
                    break;
                case StatusProposta.EmAnalise:
                    proposta.EmAnalise();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), "Status de proposta inválido.");
            }

            await _propostaRepository.UpdateAsync(proposta);
        }
    }
}