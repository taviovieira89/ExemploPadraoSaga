using Xunit;
using ContratacaoService.Domain.Entities;
using System;

namespace ContratacaoService.Domain.Tests
{
    public class ContratacaoTests
    {
        [Fact]
        public void CriarContratacao_DeveRetornarContratacaoValida()
        {
            // Arrange
            Guid propostaId = Guid.NewGuid();

            // Act
            var contratacao = new Contratacao(propostaId);

            // Assert
            Assert.NotNull(contratacao);
            Assert.Equal(propostaId, contratacao.PropostaId);
            Assert.NotEqual(default(DateTime), contratacao.DataContratacao);
        }

        [Fact]
        public void CriarContratacao_DeveLancarExcecaoParaPropostaIdVazio()
        {
            // Arrange
            Guid propostaId = Guid.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Contratacao(propostaId));
            Assert.Equal("ID da proposta é obrigatório.", exception.Message);
        }
    }
}