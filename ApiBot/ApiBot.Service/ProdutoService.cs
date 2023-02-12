using ApiBot.Domain.Models;
using ApiBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBot.Service
{
    public class ProdutoService
    {
        private readonly ProdutoRepositories _repositorio;
        public ProdutoService(ProdutoRepositories repositories)
        {
            _repositorio = repositories;
        }

        public List<Produtos> Listar(string? nomeProduto)
        {
            try
            {
                _repositorio.AbrirConexao();
                return _repositorio.ListarProdutos(nomeProduto);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Inserir()
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Inserir();
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
    }
}
