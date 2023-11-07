using NSE.BFF.Compras.Services.gRPC;
using NSE.Carrinho.API.gRPC;

namespace NSE.BFF.Compras.Configurations
{
    public static class GrpcConfig
    {
        public static IServiceCollection AddGrpcConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<GrpcServiceInterceptor>();

            services.AddScoped<ICarrinhoGrpcService, CarrinhoGrpcService>();

            services.AddGrpcClient<CarrinhoCompras.CarrinhoComprasClient>(opt =>
            {
                opt.Address = new Uri(configuration["CarrinhoUrl"]);
            }).AddInterceptor<GrpcServiceInterceptor>();

            return services;
        }
    }
}
