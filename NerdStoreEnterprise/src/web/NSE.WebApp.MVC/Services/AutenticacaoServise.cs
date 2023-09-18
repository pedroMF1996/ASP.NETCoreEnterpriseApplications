using NSE.WebApp.MVC.Models;
using System.Text;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoServise : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoServise(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponseViewModel> Login(LoginUserViewModel usuarioLogin)
        {
            var loginContent = new StringContent(JsonSerializer.Serialize(usuarioLogin), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7157/api/identidade/autenticar", loginContent);

            var options = new JsonSerializerOptions(){
                PropertyNameCaseInsensitive = true
            };

            if (!TratarErrosResponse(response))
            {
                return new LoginResponseViewModel()
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                };
            }

            return JsonSerializer.Deserialize<LoginResponseViewModel>(await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<LoginResponseViewModel> Registro(RegisterUserViewModel usuarioRegistro)
        {
            var registroContent = new StringContent(JsonSerializer.Serialize(usuarioRegistro), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7157/api/identidade/nova-conta", registroContent);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            if (!TratarErrosResponse(response))
            {
                return new LoginResponseViewModel()
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                };
            }

            return JsonSerializer.Deserialize<LoginResponseViewModel>(await response.Content.ReadAsStringAsync(), options);
        }
    }
}
