using ApiTarefas.Domain.Models;
using ApiTarefas.Domain.Validacao;
using ApiTarefas.Repositories;
using System.Text.RegularExpressions;

namespace ApiTarefas.Services
{
    public class TarefasService
    {
        private readonly TarefasRepositories _repositorio;
        public TarefasService(TarefasRepositories repositories)
        {
            _repositorio = repositories;
        }

        public List<Tarefas> ListarTarefas()
        {
            try
            {

                _repositorio.AbrirConexao();
                return _repositorio.ListarTarefas();
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public Tarefas Obter(int tarefaId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.SeExiste(tarefaId);
                return _repositorio.Obter(tarefaId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Atualizar(Tarefas model)
        {
            try
            {
                ValidarModelTarefas(model);
                _repositorio.AbrirConexao();
                _repositorio.Atualizar(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Deletar(int tarefaId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Deletar(tarefaId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Inserir(Tarefas model)
        {
            try
            {
                ValidarModelTarefas(model);
                _repositorio.AbrirConexao();
                _repositorio.InserirTarefas(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }

        private static void ValidarModelTarefas(Tarefas model)
        { 

            if (model == null)
                throw new InvalidOperationException("O Json está mal formatado");
          
                if (string.IsNullOrWhiteSpace(model.resumoTarefa))
                    throw new InvalidOperationException("Fazer um resumo da Tarefa é Obrigatório.");

                if (model.resumoTarefa.Trim().Length < 10 || model.resumoTarefa.Trim().Length > 80)
                    throw new InvalidOperationException("O Resumo precisa ter entre 10 a 80 caracteres.");
                if (string.IsNullOrWhiteSpace(model.descricaoTarefa))
                    throw new InvalidOperationException("Fazer uma Descrição Completa da Tarefa é Obrigatório.");

                if (model.descricaoTarefa.Trim().Length < 10 || model.descricaoTarefa.Trim().Length > 255)
                    throw new InvalidOperationException("A Descrição da Tarefa precisa ter entre 10 a 255 caracteres.");

            if (model.tipoTarefaId > 3 || model.tipoTarefaId <= 0)
                throw new InvalidOperationException("Há apenas 3 tipos de tarefas, selecione uma delas");

            if(!Validacao.ValidaDataHora(model.horarioInicio))
                throw new InvalidOperationException("Defina Um Horário Exato");

            if (model.horarioFim < model.horarioInicio)
                throw new InvalidOperationException("O horário Final não pode ser menor que o Inicial");
        }
    }
}

