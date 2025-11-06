using Xunit;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using System;

namespace PropostaService.Domain.Tests
{
    public class PropostaTests
    {
        [Fact]
        public void CriarProposta_DeveRetornarPropostaValida()
        {
            // Arrange
            string nomeCliente = "João Silva";
            string cpfCliente = "12345678900";
            decimal valorSeguro = 1000.00m;

            // Act
            var proposta = new Proposta(nomeCliente, cpfCliente, valorSeguro);

            // Assert
            Assert.NotNull(proposta);
            Assert.Equal(nomeCliente, proposta.NomeCliente);
            Assert.Equal(cpfCliente, proposta.CpfCliente);
            Assert.Equal(valorSeguro, proposta.ValorSeguro);
            Assert.Equal(StatusProposta.Pendente, proposta.Status);
            Assert.NotEqual(default(DateTime), proposta.DataCriacao);
        }

        [Theory]
        [InlineData("", "12345678900", 1000.00, "Nome do cliente é obrigatório.")]
        [InlineData("João Silva", "", 1000.00, "CPF do cliente é obrigatório.")]
        [InlineData("João Silva", "123", 1000.00, "CPF do cliente deve ter 11 dígitos.")]
        [InlineData("João Silva", "12345678900", 0, "Valor do seguro deve ser maior que zero.")]
        [InlineData("João Silva", "12345678900", -100, "Valor do seguro deve ser maior que zero.")]
        public void CriarProposta_DeveLancarExcecaoParaDadosInvalidos(string nomeCliente, string cpfCliente, decimal valorSeguro, string expectedErrorMessage)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Proposta(nomeCliente, cpfCliente, valorSeguro));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void AprovarProposta_DeveAlterarStatusParaAprovada()
        {
            // Arrange
            var proposta = new Proposta("João Silva", "12345678900", 1000.00m);

            // Act
            proposta.Aprovar();

            // Assert
            Assert.Equal(StatusProposta.Aprovada, proposta.Status);
        }

        [Fact]
        public void RecusarProposta_DeveAlterarStatusParaRecusada()
        {
            // Arrange
            var proposta = new Proposta("João Silva", "12345678900", 1000.00m);

            // Act
            proposta.Recusar();

            // Assert
            Assert.Equal(StatusProposta.Recusada, proposta.Status);
        }
    }
}