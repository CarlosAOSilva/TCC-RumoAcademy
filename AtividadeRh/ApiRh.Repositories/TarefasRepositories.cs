using System.Data.SqlClient;
using ApiTarefas.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace ApiTarefas.Repositories
{
    public class TarefasRepositories : Context
    {
        public TarefasRepositories(IConfiguration configuration) : base(configuration)
        {
        }

        public void InserirTarefas(Tarefas register)
        {
            string comandoSql = @"INSERT INTO Tarefa
                                    (HorarioInicio, 
                                     HorarioFim, 
                                     ResumoTarefa, 
                                     DescricaoTarefa,
                                     Cnpj,
                                     TipoTarefaId) 
                                        VALUES
                                    (@HorarioInicio, 
                                     @HorarioFim, 
                                     @ResumoTarefa, 
                                     @DescricaoTarefa,
                                     @Cnpj,
                                     @TipoTarefaId);";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@HorarioInicio", register.horarioInicio);
                cmd.Parameters.AddWithValue("@HorarioFim", register.horarioFim);
                cmd.Parameters.AddWithValue("@ResumoTarefa", register.resumoTarefa);
                cmd.Parameters.AddWithValue("@DescricaoTarefa", register.descricaoTarefa);
                cmd.Parameters.AddWithValue("@Cnpj", register.cnpj);
                cmd.Parameters.AddWithValue("@TipoTarefaId", register.tipoTarefaId);

                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Tarefas register)
        {
            string comandoSql = @"UPDATE Tarefa
                                    SET
                                     HorarioInicio = @HorarioInicio, 
                                     HorarioFim = @HorarioFim, 
                                     ResumoTarefa = @ResumoTarefa,
                                     DescricaoTarefa = @DescricaoTarefa,
                                     Cnpj = @Cnpj,
                                     TipoTarefaId = @TipoTarefaId
                                    WHERE TarefaId = @TarefaId;";


            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@TarefaId", register.tarefaId);
                cmd.Parameters.AddWithValue("@HorarioInicio", register.horarioInicio);
                cmd.Parameters.AddWithValue("@HorarioFim", register.horarioFim);
                cmd.Parameters.AddWithValue("@ResumoTarefa", register.resumoTarefa);
                cmd.Parameters.AddWithValue("@DescricaoTarefa", register.descricaoTarefa);
                cmd.Parameters.AddWithValue("@Cnpj", register.cnpj);
                cmd.Parameters.AddWithValue("@TipoTarefaId", register.tipoTarefaId);

                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para este Identificador {register.tarefaId}");
            }
        }
        public bool SeExiste(int tarefaId)
        {
            string comandoSql = @"SELECT COUNT (TarefaId) as total FROM Tarefa WHERE TarefaId = @TarefaId";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaId);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        public Tarefas Obter(int tarefaId)
        {
            string comandoSql = @"SELECT TarefaId, HorarioInicio, HorarioFim, ResumoTarefa, DescricaoTarefa, Cnpj, TipoTarefaId FROM Tarefa WHERE TarefaId = @TarefaId;";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaId);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var register = new Tarefas();

                        register.tarefaId = Convert.ToInt32(rdr["TarefaId"]);
                        register.horarioInicio = Convert.ToDateTime(rdr["HorarioInicio"]);
                        register.horarioFim = Convert.ToDateTime(rdr["HorarioFim"]);
                        register.resumoTarefa = Convert.ToString(rdr["ResumoTarefa"]);
                        register.descricaoTarefa = Convert.ToString(rdr["DescricaoTarefa"]);
                        register.cnpj= Convert.ToString(rdr["Cnpj"]);
                        register.tipoTarefaId = Convert.ToInt32(rdr["TipoTarefaId"]);
                        return register;
                    }
                    else
                        return null;
                }
            }
        }

        public List<Tarefas> ListarTarefas()
        {
            var registers = new List<Tarefas>();

            string comandoSql = @"SELECT TarefaId,
                                     HorarioInicio, 
                                     HorarioFim, 
                                     ResumoTarefa, 
                                     DescricaoTarefa,
                                     Cnpj,
                                     TipoTarefaId FROM Tarefa";


            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {

                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var register = new Tarefas();

                    register.tarefaId = Convert.ToInt32(rdr["TarefaId"]);
                    register.horarioInicio = Convert.ToDateTime(rdr["HorarioInicio"]);
                    register.horarioFim = Convert.ToDateTime(rdr["HorarioFim"]);
                    register.resumoTarefa = Convert.ToString(rdr["ResumoTarefa"]);
                    register.descricaoTarefa = Convert.ToString(rdr["DescricaoTarefa"]);
                    register.cnpj = Convert.ToString(rdr["Cnpj"]);
                    register.tipoTarefaId = Convert.ToInt32(rdr["TipoTarefaId"]);
                    registers.Add(register);
                }
                return registers;
            }


        }
        public void Deletar(int tarefaId)
        {

            string comandoSql = @"DELETE FROM Tarefa WHERE TarefaId = @TarefaId;";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaId);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para este Identificador {tarefaId}");
                else
                    cmd.ExecuteNonQuery();
            }
        }
    }
}

