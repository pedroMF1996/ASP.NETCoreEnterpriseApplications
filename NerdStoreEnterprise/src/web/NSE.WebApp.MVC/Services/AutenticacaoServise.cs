using Microsoft.Extensions.Options;
using NSE.Core.Communication;
using NSE.WebApp.MVC.Enum;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services.Interfaces;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoServise : Service, IAutenticacaoService
    {
        public AutenticacaoServise(HttpClient httpClient, IOptions<AppSettingsUrl> appSettings)
            : base(httpClient, AppSettingsUrlEnum.Identidade, appSettings)
        { }

        public async Task<LoginResponseViewModel> Login(LoginUserViewModel usuarioLogin)
        {
            var loginContent = ObterConteudo(usuarioLogin);

            var response = await _httpClient.PostAsync("/api/identidade/autenticar", loginContent);

            if (!TratarErrosResponse(response))
            {
                return new LoginResponseViewModel()
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<LoginResponseViewModel>(response);
        }

        public async Task<LoginResponseViewModel> Registro(RegisterUserViewModel usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new LoginResponseViewModel()
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<LoginResponseViewModel>(response);
        }
    }
}
