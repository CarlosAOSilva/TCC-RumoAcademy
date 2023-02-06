using ApiRh.Repositories;
using ApiRh.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApiRh.Ioc
{
    public class ContainerDependencia
    {
        public static void RegistrarServicos(IServiceCollection services)
        {
            //repositorios
            services.AddScoped<CargosRepositories, CargosRepositories>();
            services.AddScoped<EquipesRepositories, EquipesRepositories>();
            services.AddScoped<UsuarioRepositories, UsuarioRepositories>();
            services.AddScoped<FuncionariosRepositories, FuncionariosRepositories>();
            services.AddScoped<LiderancasRepositories, LiderancasRepositories>();
            services.AddScoped<PontosRepositories, PontosRepositories>();

            //services
            services.AddScoped<UsuarioService, UsuarioService>();
            services.AddScoped<AutorizationService, AutorizationService>();
            services.AddScoped<CargosService, CargosService>();
            services.AddScoped<EquipesService, EquipesService>();
            services.AddScoped<FuncionariosService, FuncionariosService>();
            services.AddScoped<LiderancasService, LiderancasService>();
            services.AddScoped<PontosService, PontosService>();
        }
    }
}

