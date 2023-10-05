using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;
using NSE.BFF.Compras.Models;
using NSE.Core.Communication;

namespace NSE.BFF.Compras.Services
{
    public interface ICarrinhoService
    {
        Task<CarrinhoDTO> ObterCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO item);
        Task<ResponseResult> AtualizarIemCarrinho(Guid idItemCarrinho, ItemCarrinhoDTO itemCarrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid idItemCarrinho);
    }
    public class CarrinhoService : Service, ICarrinhoService
    {
        public CarrinhoService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt) 
            : base(httpClient, AppSettingsUrlEnum.Carrinho, appSettingsOpt)
        {
        }

        public async Task<CarrinhoDTO> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync("/carrinho/");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CarrinhoDTO>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO item)
        {
            var itemContent = ObterConteudo(item);

            var response = await _httpClient.PostAsync("/carrinho", itemContent);

            if (!TratarErrosResponse(response))
            {
                return await DeserializarObjetoResponse<ResponseResult>(response);
            }

            return ResponderOK();
        }

        public async Task<ResponseResult> AtualizarIemCarrinho(Guid idItemCarrinho, ItemCarrinhoDTO itemCarrinho)
        {
            var itemContent = ObterConteudo(itemCarrinho);

            var response = await _httpClient.PutAsync($"/carrinho/{itemCarrinho.ProdutoId}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return ResponderOK();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid idItemCarrinho)
        {
            var response = await _httpClient.DeleteAsync($"/carrinho/{idItemCarrinho}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return ResponderOK();
        }
    }
}
