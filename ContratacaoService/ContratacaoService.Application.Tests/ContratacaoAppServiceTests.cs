using Xunit;
using Moq;
using ContratacaoService.Application.Services;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Interfaces;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Interfaces;
using PropostaService.Application.DTOs; // Assuming this DTO is used for the HTTP client
using System.Threading.Tasks;
using System;

namespace ContratacaoService.Application.Tests
{
    public class ContratacaoAppServiceTests
    {
        private readonly Mock<IContratacaoRepository> _mockContratacaoRepository;
        private readonly Mock<IPropostaServiceHttpClient> _mockPropostaServiceHttpClient;
        private readonly IContratacaoAppService _contratacaoAppService;

        public ContratacaoAppServiceTests()
        {
            _mockContratacaoRepository = new Mock<IContratacaoRepository>();
            _mockPropostaServiceHttpClient = new Mock<IPropostaServiceHttpClient>();
            _contratacaoAppService = new ContratacaoAppService(
                _mockContratacaoRepository.Object,
                _mockPropostaServiceHttpClient.Object
            );
        }

        [Fact]
        public async Task ContratarProposta_DeveRetornarContratacaoDTO_QuandoPropostaAprovada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var contratarPropostaDTO = new ContratarPropostaDTO { PropostaId = propostaId };
            var propostaDTO = new PropostaDTO { Id = propostaId, Status = PropostaService.Domain.Enums.StatusProposta.Aprovada };

            _mockPropostaServiceHttpClient.Setup(client => client.AprovarProposta(propostaId))
                .ReturnsAsync(propostaDTO);
            _mockContratacaoRepository.Setup(repo => repo.AddAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync((Contratacao c) =>
                {
                    c.Id = Guid.NewGuid();
                    return c;
                });

            // Act
            var result = await _contratacaoAppService.ContratarProposta(contratarPropostaDTO);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(propostaId, result.PropostaId);
            _mockPropostaServiceHttpClient.Verify(client => client.AprovarProposta(propostaId), Times.Once);
            _mockContratacaoRepository.Verify(repo => repo.AddAsync(It.IsAny<Contratacao>()), Times.Once);
        }

        [Fact]
        public async Task ContratarProposta_DeveRetornarNull_QuandoPropostaNaoAprovada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var contratarPropostaDTO = new ContratarPropostaDTO { PropostaId = propostaId };
            var propostaDTO = new PropostaDTO { Id = propostaId, Status = PropostaService.Domain.Enums.StatusProposta.Recusada };

            _mockPropostaServiceHttpClient.Setup(client => client.AprovarProposta(propostaId))
                .ReturnsAsync(propostaDTO);

            // Act
            var result = await _contratacaoAppService.ContratarProposta(contratarPropostaDTO);

            // Assert
            Assert.Null(result);
            _mockPropostaServiceHttpClient.Verify(client => client.AprovarProposta(propostaId), Times.Once);
            _mockContratacaoRepository.Verify(repo => repo.AddAsync(It.IsAny<Contratacao>()), Times.Never);
        }

        [Fact]
        public async Task ContratarProposta_DeveRetornarNull_QuandoPropostaNaoEncontrada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var contratarPropostaDTO = new ContratarPropostaDTO { PropostaId = propostaId };

            _mockPropostaServiceHttpClient.Setup(client => client.AprovarProposta(propostaId))
                .ReturnsAsync((PropostaDTO)null);

            // Act
            var result = await _contratacaoAppService.ContratarProposta(contratarPropostaDTO);

            // Assert
            Assert.Null(result);
            _mockPropostaServiceHttpClient.Verify(client => client.AprovarProposta(propostaId), Times.Once);
            _mockContratacaoRepository.Verify(repo => repo.AddAsync(It.IsAny<Contratacao>()), Times.Never);
        }
    }
}