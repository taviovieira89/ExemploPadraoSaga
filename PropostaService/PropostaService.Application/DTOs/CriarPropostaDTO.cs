namespace PropostaService.Application.DTOs
{
    public class CriarPropostaDTO
    {
        public string NomeCliente { get; set; }
        public string CpfCliente { get; set; }
        public decimal ValorSeguro { get; set; }
    }
}