namespace ContratacaoService.Application.DTOs
{
    public class ContratacaoDTO
    {
        public Guid Id { get; set; }
        public Guid PropostaId { get; set; }
        public DateTime DataContratacao { get; set; }
    }
}