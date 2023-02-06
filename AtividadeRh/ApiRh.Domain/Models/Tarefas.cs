namespace ApiTarefas.Domain.Models
{
    public class Tarefas
    {
        public int tarefaId { get; set; }
        public DateTime horarioInicio { get; set; }
        public DateTime horarioFim { get; set; }

        public string resumoTarefa { get; set; }

        public string descricaoTarefa { get; set; }

        public int tipoTarefaId { get; set; }

        public string cnpj { get; set; }


    }   
            
}
