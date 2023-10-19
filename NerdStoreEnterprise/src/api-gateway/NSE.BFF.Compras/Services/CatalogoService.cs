using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;
using NSE.BFF.Compras.Models;
using NuGet.Packaging.Signing;

namespace NSE.BFF.Compras.Services
{
    public interface ICatalogoService
    {
        Task<IEnumerable<ItemProdutoDTO>> ObterTodos();
        Task<ItemProdutoDTO> ObterPorId(Guid id);
        Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids);
    }
    public class CatalogoService : Service, ICatalogoService
    {
        public CatalogoService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt) 
            : base(httpClient, AppSettingsUrlEnum.Catalogo, appSettingsOpt)
        {
        }

        public async Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids)
        {
            var idsRequest = string.Join(",", ids);

            var response = await _httpClient.GetAsync($"/catalogo/produtos/lista/{idsRequest}/");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<ItemProdutoDTO>>(response);
        }

        public async Task<ItemProdutoDTO> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ItemProdutoDTO>(response);
        }

        public async Task<IEnumerable<ItemProdutoDTO>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/catalogo/produtos");
             
            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<ItemProdutoDTO>>(response);
        }
    }
}
