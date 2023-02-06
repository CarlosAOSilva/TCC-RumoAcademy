namespace ApiTarefas.Domain.Models
{

    public enum EnumCargoUsuario
    {
        Desenvolvedor = 1,
        Gestor = 2
       
    }
    public class Usuario
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public EnumCargoUsuario CargoUsuario { get; set; }
    }
}


