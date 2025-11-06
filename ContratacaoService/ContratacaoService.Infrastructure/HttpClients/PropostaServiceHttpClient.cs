using ContratacaoService.Application.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContratacaoService.Infrastructure.HttpClients
{
    public class PropostaServiceHttpClient : IPropostaServiceHttpClient
    {
        private readonly HttpClient _httpClient;

        public PropostaServiceHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PropostaStatusDTO> GetPropostaStatusAsync(Guid propostaId)
        {
            var response = await _httpClient.GetAsync($"/api/propostas/{propostaId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var proposta = JsonSerializer.Deserialize<PropostaStatusDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return proposta;
        }

        public async Task UpdatePropostaStatusAsync(Guid propostaId, string status)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { status }), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/propostas/{propostaId}/status?status={status}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}