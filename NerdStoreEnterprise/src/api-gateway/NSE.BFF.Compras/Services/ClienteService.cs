using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;
using NSE.BFF.Compras.Models;
using System.Net;

namespace NSE.BFF.Compras.Services
{
    public interface IClienteService
    {
        Task<EnderecoDTO> ObterEndereco();
    }
    public class ClienteService : Service, IClienteService
    {
        public ClienteService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt) :
            base(httpClient, AppSettingsUrlEnum.Cliente, appSettingsOpt)
        {
        }

        public async Task<EnderecoDTO> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/cliente/endereco/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EnderecoDTO>(response);
        }
    }
}
