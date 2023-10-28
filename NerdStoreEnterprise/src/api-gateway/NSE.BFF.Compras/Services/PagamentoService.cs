using Microsoft.Extensions.Options;
using NSE.BFF.Compras.Enum;
using NSE.BFF.Compras.Extensions;

namespace NSE.BFF.Compras.Services
{
    public interface IPagamentoSerive
    {

    }
    public class PagamentoService : Service, IPagamentoSerive
    {
        public PagamentoService(HttpClient httpClient, IOptions<AppServiceSettings> appSettingsOpt)
            : base(httpClient, AppSettingsUrlEnum.Pagamento, appSettingsOpt)
        {
        }
    }
}
