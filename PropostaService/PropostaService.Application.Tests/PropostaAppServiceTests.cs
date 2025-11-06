using Xunit;
using Moq;
using PropostaService.Application.Services;
using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Interfaces;
using PropostaService.Domain.Enums;
using System.Threading.Tasks;
using System;

namespace PropostaService.Application.Tests
{
    public class PropostaAppServiceTests
    {
        private readonly Mock<IPropostaRepository> _mockPropostaRepository;
        private readonly PropostaAppService _propostaAppService;

        public PropostaAppServiceTests()
        {
            _mockPropostaRepository = new Mock<IPropostaRepository>();
            _propostaAppService = new PropostaAppService(_mockPropostaRepository.Object);
        }

        [Fact]
        public async Task CriarProposta_DeveRetornarPropostaDTO()
        {
            // Arrange
            var criarPropostaDTO = new CriarPropostaDTO
            {
                NomeCliente = "Maria Souza",
                CpfCliente = "09876543210",
                ValorSeguro = 2500.00m
            };

            _mockPropostaRepository.Setup(repo => repo.AddAsync(It.IsAny<Proposta>()))
                .ReturnsAsync((Proposta p) =>
                {
                    p.Id = Guid.NewGuid();
                    return p;
                });

            // Act
            var result = await _propostaAppService.CriarProposta(criarPropostaDTO);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(criarPropostaDTO.NomeCliente, result.NomeCliente);
            Assert.Equal(criarPropostaDTO.CpfCliente, result.CpfCliente);
            Assert.Equal(criarPropostaDTO.ValorSeguro, result.ValorSeguro);
            Assert.Equal(StatusProposta.Pendente, result.Status);
            _mockPropostaRepository.Verify(repo => repo.AddAsync(It.IsAny<Proposta>()), Times.Once);
        }

        [Fact]
        public async Task AprovarProposta_DeveRetornarPropostaAprovada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var proposta = new Proposta("Cliente Teste", "11122233344", 1500.00m) { Id = propostaId, Status = StatusProposta.Pendente };

            _mockPropostaRepository.Setup(repo => repo.GetByIdAsync(propostaId))
                .ReturnsAsync(proposta);
            _mockPropostaRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Proposta>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _propostaAppService.AprovarProposta(propostaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusProposta.Aprovada, result.Status);
            _mockPropostaRepository.Verify(repo => repo.GetByIdAsync(propostaId), Times.Once);
            _mockPropostaRepository.Verify(repo => repo.UpdateAsync(It.Is<Proposta>(p => p.Id == propostaId && p.Status == StatusProposta.Aprovada)), Times.Once);
        }

        [Fact]
        public async Task AprovarProposta_DeveRetornarNullSeNaoEncontrada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            _mockPropostaRepository.Setup(repo => repo.GetByIdAsync(propostaId))
                .ReturnsAsync((Proposta)null);

            // Act
            var result = await _propostaAppService.AprovarProposta(propostaId);

            // Assert
            Assert.Null(result);
            _mockPropostaRepository.Verify(repo => repo.GetByIdAsync(propostaId), Times.Once);
            _mockPropostaRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Proposta>()), Times.Never);
        }

        [Fact]
        public async Task RecusarProposta_DeveRetornarPropostaRecusada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var proposta = new Proposta("Cliente Teste", "11122233344", 1500.00m) { Id = propostaId, Status = StatusProposta.Pendente };

            _mockPropostaRepository.Setup(repo => repo.GetByIdAsync(propostaId))
                .ReturnsAsync(proposta);
            _mockPropostaRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Proposta>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _propostaAppService.RecusarProposta(propostaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusProposta.Recusada, result.Status);
            _mockPropostaRepository.Verify(repo => repo.GetByIdAsync(propostaId), Times.Once);
            _mockPropostaRepository.Verify(repo => repo.UpdateAsync(It.Is<Proposta>(p => p.Id == propostaId && p.Status == StatusProposta.Recusada)), Times.Once);
        }

        [Fact]
        public async Task RecusarProposta_DeveRetornarNullSeNaoEncontrada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            _mockPropostaRepository.Setup(repo => repo.GetByIdAsync(propostaId))
                .ReturnsAsync((Proposta)null);

            // Act
            var result = await _propostaAppService.RecusarProposta(propostaId);

            // Assert
            Assert.Null(result);
            _mockPropostaRepository.Verify(repo => repo.GetByIdAsync(propostaId), Times.Once);
            _mockPropostaRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Proposta>()), Times.Never);
        }
    }
}