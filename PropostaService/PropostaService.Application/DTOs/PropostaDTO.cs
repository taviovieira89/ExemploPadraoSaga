using PropostaService.Domain.Enums;

namespace PropostaService.Application.DTOs
{
    public class PropostaDTO
    {
        public Guid Id { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCliente { get; set; }
        public decimal ValorSeguro { get; set; }
        public StatusProposta Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}