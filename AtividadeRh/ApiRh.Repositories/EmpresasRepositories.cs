using System.Data.SqlClient;
using ApiTarefas.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace ApiTarefas.Repositories
{
    public class EmpresasRepositories : Context
    {
        public EmpresasRepositories(IConfiguration configuration) : base(configuration)
        {
        }

        public void InserirEmpresas(Empresas register)
        {
            string comandoSql = @"INSERT INTO Empresa
                                    (Cnpj,
                                      RazaoSocial,
                                      DataCadastro) 
                                        VALUES
                                     (@Cnpj,
                                      @RazaoSocial,
                                      @DataCadastro);";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@Cnpj", register.cnpj);
                cmd.Parameters.AddWithValue("@RazaoSocial", register.razaoSocial);
                cmd.Parameters.AddWithValue("@DataCadastro", register.dataCadastro);

                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Empresas register)
        {
            string comandoSql = @"UPDATE Empresa
                                    SET
                                      RazaoSocial = @RazaoSocial, DataCadastro = @DataCadastro
                                    WHERE Cnpj = @Cnpj;";


            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@Cnpj", register.cnpj);
                cmd.Parameters.AddWithValue("@RazaoSocial", register.razaoSocial);
                cmd.Parameters.AddWithValue("@DataCadastro", register.dataCadastro);

                cmd.ExecuteNonQuery();

                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para este CNPJ {register.cnpj}");
            }
        }
        public bool SeExiste(string? cnpj)
        {
            string comandoSql = @"SELECT COUNT (Cnpj) FROM Empresa WHERE Cnpj = @Cnpj";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@Cnpj", cnpj);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        public Empresas Obter(string? cnpj)
        {
            string comandoSql = @"SELECT Cnpj, RazaoSocial, DataCadastro
                                     FROM Empresa WHERE Cnpj = @Cnpj";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@Cnpj", cnpj);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var register = new Empresas();

                        register.cnpj = Convert.ToString(rdr["Cnpj"]);
                        register.razaoSocial = Convert.ToString(rdr["RazaoSocial"]);
                        register.dataCadastro = Convert.ToDateTime(rdr["DataCadastro"]);

                        return register;
                    }
                    else
                        return null;
                }
            }
        }
        public List<Empresas> ListarEmpresas(string? cnpj)
        {
            string comandoSql = @"SELECT Cnpj, RazaoSocial, DataCadastro
                                     FROM Empresa";

            if (!string.IsNullOrWhiteSpace(cnpj))
                comandoSql += " WHERE cnpj LIKE @cnpj";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                if (!string.IsNullOrWhiteSpace(cnpj))
                    cmd.Parameters.AddWithValue("@Cnpj", "%" + cnpj + "%");

                using (var rdr = cmd.ExecuteReader())
                {
                    var registers = new List<Empresas>();
                    while (rdr.Read())
                    {
                        var register = new Empresas();

                        register.cnpj = Convert.ToString(rdr["Cnpj"]);
                        register.razaoSocial = Convert.ToString(rdr["RazaoSocial"]);
                        register.dataCadastro = Convert.ToDateTime(rdr["DataCadastro"]);

                        registers.Add(register);
                    }
                    return registers;
                }
            }
        }
        public void Deletar(string cnpj)
        {
            string comandoSql = @"DELETE FROM Empresa WHERE Cnpj = @Cnpj;";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@CNPJ", cnpj);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para este CNPJ {cnpj}");
                else
                    cmd.ExecuteNonQuery();
            }
        }
    }
}
