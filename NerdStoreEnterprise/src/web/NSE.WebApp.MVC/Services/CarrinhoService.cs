using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Enum;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Interfaces;

namespace NSE.WebApp.MVC.Services
{
    public class CarrinhoService : Service, ICarrinhoService
    {
        public CarrinhoService(HttpClient httpClient, IOptions<AppSettingsUrl> appSettingsOpt) : 
            base(httpClient, AppSettingsUrlEnum.Carrinho, appSettingsOpt)
        {}

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync("/carrinho");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(CarrinhoViewModel item)
        {
            var itemContent = ObterConteudo(item);
            
            var response = await _httpClient.PostAsync("/carrinho/", itemContent);

            if(!TratarErrosResponse(response))
            {
                return await DeserializarObjetoResponse<ResponseResult>(response);
            }

            return ResponderOK();
        }

        public async Task<ResponseResult> AtualizarIemCarrinho(Guid idItemCarrinho, ItemCarrinhoViewModel itemCarrinho)
        {
            var itemContent = ObterConteudo(itemCarrinho);

            var response = await _httpClient.PutAsync($"/carrinho/{idItemCarrinho}", itemContent);

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
