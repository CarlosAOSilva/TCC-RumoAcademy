using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBot.Repositories
{
    public class Context
    {
        public readonly SqlConnection _conexao;
        public Context(IConfiguration configuration)
        {
            _conexao = new SqlConnection(configuration["DbCredentials"]);
        }
        public void AbrirConexao()
        {
            _conexao.Open();
        }

        public void FecharConexao()
        {
            _conexao.Close();
        }
    }
}
