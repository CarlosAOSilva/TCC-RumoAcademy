using System.Data.SqlClient;
using ApiTarefas.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace ApiTarefas.Repositories
{
    public class UsuarioRepositories : Context
    {
        public UsuarioRepositories(IConfiguration configuration) : base(configuration)
        {
        }

        public Usuario? ObterUsuarioPorCredenciais(string email, string senha)
        {
            string comandoSql = @"SELECT u.Email, u.Nome, u.CargoId FROM Usuario u
                                    JOIN Cargos c ON u.CargoId = c.CargoId
                                    WHERE u.Email = @email AND u.Senha = @senha";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@senha", senha);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        return new Usuario()
                        {
                            Nome = rdr["Nome"].ToString(),
                            Email = rdr["Email"].ToString(),
                            CargoUsuario = (EnumCargoUsuario)Convert.ToInt32(rdr["CargoId"])
                        };
                    }
                    else
                        return null;
                }
            }
        }
    }
}
