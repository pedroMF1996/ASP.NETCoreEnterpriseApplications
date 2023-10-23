using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;
using NSE.Core.Communication;
using System.Net;
using System.Text;
using System.Text.Json;

namespace NSE.BFF.Compras.Services
{
    public abstract class Service
    {
        protected readonly HttpClient _httpClient;

        protected Service(HttpClient httpClient, AppSettingsUrlEnum appSettingsUrlEnum, IOptions<AppServiceSettings> appSettingsOpt)
        {
            _httpClient = httpClient;
            switch (appSettingsUrlEnum)
            {
                case AppSettingsUrlEnum.Catalogo:
                    _httpClient.BaseAddress = new Uri(appSettingsOpt.Value.CatalogoUrl);
                    break;
                case AppSettingsUrlEnum.Carrinho:
                    _httpClient.BaseAddress = new Uri(appSettingsOpt.Value.CarrinhoUrl);
                    break;
                case AppSettingsUrlEnum.Pedido:
                    _httpClient.BaseAddress = new Uri(appSettingsOpt.Value.PedidoUrl);
                    break;
                case AppSettingsUrlEnum.Pagamento:
                    _httpClient.BaseAddress = new Uri(appSettingsOpt.Value.PagamentoUrl);
                    break;
                case AppSettingsUrlEnum.Cliente:
                    _httpClient.BaseAddress = new Uri(appSettingsOpt.Value.ClienteUrl);
                    break;
            }
        }

        protected StringContent ObterConteudo(object dado)
        {
            return new StringContent(JsonSerializer.Serialize(dado), Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest) return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected ResponseResult ResponderOK()
        {
            return new ResponseResult();
        }
    }
}
