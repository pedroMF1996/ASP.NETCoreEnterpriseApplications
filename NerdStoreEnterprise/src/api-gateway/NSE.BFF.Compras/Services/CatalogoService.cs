using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;
using NSE.BFF.Compras.Models;

namespace NSE.BFF.Compras.Services
{
    public interface ICatalogoService
    {
        Task<IEnumerable<ItemProdutoDTO>> ObterTodos();
        Task<ItemProdutoDTO> ObterPorId(Guid id);
    }
    public class CatalogoService : Service, ICatalogoService
    {
        public CatalogoService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt) 
            : base(httpClient, AppSettingsUrlEnum.Catalogo, appSettingsOpt)
        {
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
