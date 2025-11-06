using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Interfaces;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Interfaces;

namespace ContratacaoService.Application.Services
{
    public class ContratacaoAppService : IContratacaoAppService
    {
        private readonly IContratacaoRepository _contratacaoRepository;
        private readonly IPropostaServiceHttpClient _propostaServiceHttpClient;

        public ContratacaoAppService(IContratacaoRepository contratacaoRepository, IPropostaServiceHttpClient propostaServiceHttpClient)
        {
            _contratacaoRepository = contratacaoRepository;
            _propostaServiceHttpClient = propostaServiceHttpClient;
        }

        public async Task<ContratacaoDTO> ContratarPropostaAsync(ContratarPropostaDTO contratarPropostaDTO)
        {
            var propostaStatus = await _propostaServiceHttpClient.GetPropostaStatusAsync(contratarPropostaDTO.PropostaId);

            if (propostaStatus == null)
            {
                throw new KeyNotFoundException($"Proposta com ID {contratarPropostaDTO.PropostaId} não encontrada.");
            }

            if (propostaStatus.Status != "Aprovada")
            {
                throw new InvalidOperationException($"A proposta com ID {contratarPropostaDTO.PropostaId} não está aprovada para contratação. Status atual: {propostaStatus.Status}");
            }

            var contratacao = new Contratacao(contratarPropostaDTO.PropostaId);
            await _contratacaoRepository.AddAsync(contratacao);

            // Opcional: Atualizar o status da proposta no PropostaService para "Contratada"
            // await _propostaServiceHttpClient.UpdatePropostaStatusAsync(contratarPropostaDTO.PropostaId, "Contratada");

            return new ContratacaoDTO
            {
                Id = contratacao.Id,
                PropostaId = contratacao.PropostaId,
                DataContratacao = contratacao.DataContratacao
            };
        }

        public async Task<ContratacaoDTO> GetContratacaoByIdAsync(Guid id)
        {
            var contratacao = await _contratacaoRepository.GetByIdAsync(id);
            if (contratacao == null) return null;

            return new ContratacaoDTO
            {
                Id = contratacao.Id,
                PropostaId = contratacao.PropostaId,
                DataContratacao = contratacao.DataContratacao
            };
        }

        public async Task<IEnumerable<ContratacaoDTO>> GetAllContratacoesAsync()
        {
            var contratacoes = await _contratacaoRepository.GetAllAsync();
            return contratacoes.Select(c => new ContratacaoDTO
            {
                Id = c.Id,
                PropostaId = c.PropostaId,
                DataContratacao = c.DataContratacao
            });
        }
    }
}