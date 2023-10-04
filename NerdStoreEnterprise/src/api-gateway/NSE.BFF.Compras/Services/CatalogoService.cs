using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;

namespace NSE.BFF.Compras.Services
{
    public interface ICatalogoService
    {

    }
    public class CatalogoService : Service, ICatalogoService
    {
        public CatalogoService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt) 
            : base(httpClient, AppSettingsUrlEnum.Catalogo, appSettingsOpt)
        {
        }
    }
}
