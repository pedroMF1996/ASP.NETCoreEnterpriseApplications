using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;

namespace NSE.BFF.Compras.Services
{
    public interface IPedidoService
    {

    }
    public class PedidoService : Service, IPedidoService
    {
        public PedidoService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt) 
            : base(httpClient, AppSettingsUrlEnum.Pedido, appSettingsOpt)
        {
        }
    }
}
