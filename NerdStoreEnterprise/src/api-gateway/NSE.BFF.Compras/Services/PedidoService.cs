using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;
using NSE.BFF.Compras.Models;
using NSE.Core.Communication;
using System.Net;

namespace NSE.BFF.Compras.Services
{
    public interface IPedidoService
    {
        Task<ResponseResult> FinalizarPedido(PedidoTransacaoDTO pedido);
        Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId();
        Task<PedidoDTO> ObterUltimoPedido();
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }
    public class PedidoService : Service, IPedidoService
    {
        public PedidoService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt)
            : base(httpClient, AppSettingsUrlEnum.Pedido, appSettingsOpt)
        {
        }

        public async Task<ResponseResult> FinalizarPedido(PedidoTransacaoDTO pedido)
        {
            var pedidoContent = ObterConteudo(pedido);

            var response = await _httpClient.PostAsync("/pedido/", pedidoContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return ResponderOK();
        }

        public async Task<PedidoDTO> ObterUltimoPedido()
        {
            var response = await _httpClient.GetAsync("/pedido/ultimo/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PedidoDTO>(response);
        }

        public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId()
        {
            var response = await _httpClient.GetAsync("/pedido/lista-cliente/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<PedidoDTO>>(response);
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"/voucher/{codigo}/");

            if (HttpStatusCode.NotFound == response.StatusCode) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VoucherDTO>(response);
        }
    }
}
