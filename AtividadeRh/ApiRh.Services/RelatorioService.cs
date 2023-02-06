using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTarefas.Services
{
    public class RelatorioService
    {
        private readonly TarefasService _tarefaService;
        public RelatorioService(TarefasService tarefaservice)
        {
            _tarefaService = tarefaservice;
        }
        public Stream GerarRelatorioTarefas()
        {
            var tarefas = _tarefaService.ListarTarefas();

            string conteudoCsv = "Identificador Tarefa ; Horario de Inicio; Horario de Finalizacao ; Resumo Tarefa; Descricao da Tarefa; Cnpj; Tipo de Tarefa;" + Environment.NewLine;

            foreach (var tarefa in tarefas)
            {
                conteudoCsv += string.Format("{0};{1};{2};{3};{4};{5};{6};{7}"
                    , tarefa.tarefaId
                    , tarefa.horarioInicio
                    , tarefa.horarioFim
                    , tarefa.resumoTarefa
                    , tarefa.descricaoTarefa
                    , tarefa.cnpj
                    , tarefa.tipoTarefaId
                    , Environment.NewLine);
            }

            return GenerateStreamFromString(conteudoCsv);
        }
        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}



