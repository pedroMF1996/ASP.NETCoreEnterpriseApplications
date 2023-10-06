using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;
using NSE.BFF.Compras.Models;
using System.Net;

namespace NSE.BFF.Compras.Services
{
    public interface IPedidoService
    {
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }
    public class PedidoService : Service, IPedidoService
    {
        public PedidoService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt) 
            : base(httpClient, AppSettingsUrlEnum.Pedido, appSettingsOpt)
        {
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"/voucher/{codigo}/");

            if(HttpStatusCode.NotFound == response.StatusCode) return null; 

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VoucherDTO>(response);
        }
    }
}
