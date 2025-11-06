using PropostaService.Domain.Enums;

namespace PropostaService.Domain.Entities
{
    public class Proposta
    {
        public Guid Id { get; private set; }
        public string NomeCliente { get; private set; }
        public string CpfCliente { get; private set; }
        public decimal ValorSeguro { get; private set; }
        public StatusProposta Status { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public Proposta(string nomeCliente, string cpfCliente, decimal valorSeguro)
        {
            Id = Guid.NewGuid();
            NomeCliente = nomeCliente;
            CpfCliente = cpfCliente;
            ValorSeguro = valorSeguro;
            Status = StatusProposta.EmAnalise;
            DataCriacao = DateTime.UtcNow;
        }

        public void Aprovar()
        {
            if (Status != StatusProposta.EmAnalise)
            {
                throw new InvalidOperationException("A proposta só pode ser aprovada se estiver 'Em Análise'.");
            }
            Status = StatusProposta.Aprovada;
        }

        public void Rejeitar()
        {
            if (Status != StatusProposta.EmAnalise)
            {
                throw new InvalidOperationException("A proposta só pode ser rejeitada se estiver 'Em Análise'.");
            }
            Status = StatusProposta.Rejeitada;
        }

        public void EmAnalise()
        {
            Status = StatusProposta.EmAnalise;
        }
    }
}