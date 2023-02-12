using ApiBot.Repositories;
using ApiBot.Service;

namespace ApiBot
{
    public class ContainerDepedencias
    {
        public static void RegistrarServicos(IServiceCollection services)
        {
            //repositorios
            services.AddScoped<ProdutoRepositories, ProdutoRepositories>();

            //services
            services.AddScoped<ProdutoService, ProdutoService>();
        }
    }
}
