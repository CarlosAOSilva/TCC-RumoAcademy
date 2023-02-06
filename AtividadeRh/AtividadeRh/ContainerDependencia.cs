using ApiTarefas.Repositories;
using ApiTarefas.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApiSistemaRegistro
{
    public class ContainerDependencia
    {
        public static void RegistrarServicos(IServiceCollection services)
        {
            //repositorios
            services.AddScoped<EmpresasRepositories, EmpresasRepositories>();
            services.AddScoped<UsuarioRepositories, UsuarioRepositories>();
            services.AddScoped<TarefasRepositories, TarefasRepositories>();

            //services
            services.AddScoped<UsuarioService, UsuarioService>();
            services.AddScoped<AutorizationService, AutorizationService>();
            services.AddScoped<EmpresasService, EmpresasService>();
            services.AddScoped<TarefasService, TarefasService>();
            services.AddScoped<RelatorioService, RelatorioService>();
        }
    }
}

