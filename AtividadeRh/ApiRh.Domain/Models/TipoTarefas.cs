namespace ApiTarefas.Domain.Models
{
    public enum EnumTipoTarefa
    {
        Reuniao = 1,
        QuebraDeContexto = 2,
        Tarefa = 3,

    }
    public class TipoTarefas
    {
        public int tipoTarefaId { get; set; }
        public string? nomeTarefa { get; set; }

        public EnumTipoTarefa tipoTarefa { get; set; }
    }
}
