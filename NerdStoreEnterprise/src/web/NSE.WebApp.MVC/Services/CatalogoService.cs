using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Enum;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Interfaces;

namespace NSE.WebApp.MVC.Services
{
    public class CatalogoService : Service, ICatalogoService
    {
        public CatalogoService(HttpClient httpClient, IOptions<AppSettingsUrl> appSettingsOpt)
            : base(httpClient, AppSettingsUrlEnum.Catalogo, appSettingsOpt)
        { }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }

        public async Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync(string.Format("/catalogo/produtos?ps={0}&page={1}&q={2}", pageSize, pageIndex, query));

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PagedViewModel<ProdutoViewModel>>(response);
        }
    }
}
