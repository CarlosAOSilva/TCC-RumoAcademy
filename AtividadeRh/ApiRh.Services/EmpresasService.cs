using ApiTarefas.Domain.Models;
using ApiTarefas.Domain.Validacao;
using ApiTarefas.Repositories;
using System.Text.RegularExpressions;

namespace ApiTarefas.Services
{

    public class EmpresasService
    {
        private readonly EmpresasRepositories _repositorio;
        public EmpresasService(EmpresasRepositories repositories)
        {
            _repositorio = repositories;
        }

        public List<Empresas> ListarEmpresas(string? cnpj)
        {
            try
            {
                _repositorio.AbrirConexao();
                return _repositorio.ListarEmpresas(cnpj);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public Empresas Obter(string? cnpj)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.SeExiste(cnpj);
                return _repositorio.Obter(cnpj);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Atualizar(Empresas model)
        {
            try
            {
                ValidarModelEmpresas(model);
                _repositorio.AbrirConexao();
                _repositorio.Atualizar(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Deletar(string? cnpj)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Deletar(cnpj);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Inserir(Empresas model)
        {
            try
            {
                ValidarModelEmpresas(model);
                _repositorio.AbrirConexao();
                _repositorio.InserirEmpresas(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        private static void ValidarModelEmpresas(Empresas model, bool isUpdate = false)
        {

            if (model == null)
                throw new InvalidOperationException("O Json está mal formatado");
            if (!isUpdate)
            {
                if (string.IsNullOrWhiteSpace(model.cnpj))
                    throw new InvalidOperationException("O CNPJ é Obrigatório.");

                if (!Validacao.IsCnpj(model.cnpj))
                    throw new InvalidOperationException("CNPJ Inválido.");
            }
            if (string.IsNullOrWhiteSpace(model.razaoSocial))
                throw new InvalidOperationException("Razão Social é Obrigatória.");

            if (model.razaoSocial.Trim().Length < 3 || model.razaoSocial.Trim().Length > 80)
                throw new InvalidOperationException("A Razão Social precisa ter entre 3 a 80 caracteres.");
        }
    }
}

