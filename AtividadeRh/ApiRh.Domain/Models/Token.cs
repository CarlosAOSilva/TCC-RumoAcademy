namespace ApiTarefas.Domain.Models
{

    public class Token
    {
        public string Bearer { get; set; }
        public DateTime Validade { get; set; }
        public int NivelAcesso { get; set; }
        public string NomeUsuario { get; set; }
    }
}


