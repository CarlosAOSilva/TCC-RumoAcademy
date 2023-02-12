using ApiBot.Domain.Models;
using Microsoft.Extensions.Configuration;
using ProdutoBot;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBot.Repositories
{
    public class ProdutoRepositories : Context
    {
        CultureInfo culture = new CultureInfo("pt-BR");

        private readonly Bot _bot;
        public ProdutoRepositories(IConfiguration configuration) : base(configuration)
        {
            _bot = new Bot("https://www.bazardebagda.com.br");
        }
        public List<Produtos> ListarProdutos(string? nomeProduto)
        {
            string comandoSql = @"SELECT NomeProduto, PrecoAntigo, PrecoAtual, DataConsulta, SiteProduto FROM Produtos";

            if (!string.IsNullOrWhiteSpace(nomeProduto))
                comandoSql = " WHERE NomeProduto LIKE @NomeProduto";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                if (!string.IsNullOrWhiteSpace(nomeProduto))
                    cmd.Parameters.AddWithValue("@NomeProduto", "%" + nomeProduto + "%");

                using (var rdr = cmd.ExecuteReader())
                {
                    var registers = new List<Produtos>();
                    while (rdr.Read())
                    {
                        var register = new Produtos();
                        register.nomeProduto = Convert.ToString(rdr["NomeProduto"]);
                        register.precoAntigo = Convert.ToDecimal(rdr["PrecoAntigo"]);
                        register.precoAtual = Convert.ToDecimal(rdr["PrecoAtual"]);
                        register.dataConsulta = Convert.ToDateTime(rdr["DataConsulta"]);
                        register.siteProduto = Convert.ToString(rdr["SiteProduto"]);
                        registers.Add(register);
                    }
                    return registers;
                }
            }
        }

        public void Inserir()
        {
            var produtos = _bot.Obter().ToList();

            foreach (var produto in produtos)
            {
                if (SeExiste(produto.nomeProduto, produto.precoAtual))
                {
                    Atualizar(produto);
                }
                else
                {
                    InserirProdutos(produto);
                }

            }
        }
        private bool SeExiste(string nomeProduto, decimal precoAtual)
        {
            string comandoSql = "SELECT COUNT (*) FROM Produtos WHERE  NomeProduto = @NomeProduto AND PrecoAtual = @PrecoAtual";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@NomeProduto", nomeProduto);
                cmd.Parameters.AddWithValue("@PrecoAtual", precoAtual);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
        private void Atualizar(Produtos register)
        {
            string comandoSql = @"UPDATE Produtos
                                    SET
                                     NomeProduto = @NomeProduto,
                                     PrecoAntigo = @PrecoAntigo,
                                     PrecoAtual = @PrecoAtual,
                                     DataConsulta = @DataConsulta,
                                     SiteProduto = @SiteProduto
                                    WHERE NomeProduto = @NomeProduto";

            decimal precoAntigo = ObterValor(register.nomeProduto);

            if (precoAntigo != register.precoAtual)
            {
                using (var cmd = new SqlCommand(comandoSql, _conexao))
                {
                    cmd.Parameters.AddWithValue("@NomeProduto", register.nomeProduto);
                    cmd.Parameters.AddWithValue("@PrecoAntigo", register.precoAntigo);
                    cmd.Parameters.AddWithValue("@PrecoAtual", register.precoAtual);
                    cmd.Parameters.AddWithValue("@DataConsulta", register.dataConsulta);
                    cmd.Parameters.AddWithValue("@SiteProduto", register.siteProduto);

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException($"Nenhum registro afetado para este produto: {register.nomeProduto}");
                }
            }
        }

        private decimal ObterValor(string nomeProduto)
        {
            string comandoSql = @"SELECT PrecoAtual FROM Produtos WHERE NomeProduto = @NomeProduto;";
            decimal precoAtual = 0;
            using (SqlCommand cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@NomeProduto", nomeProduto);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        precoAtual = Convert.ToDecimal(rdr["PrecoAtual"]);
                    }
                }
            }
            return precoAtual;
        }
        private void InserirProdutos(Produtos register)
        {
            string comandoSql = @"INSERT INTO Produtos
                                        (NomeProduto,
                                         PrecoAntigo,
                                         PrecoAtual,
                                         DataConsulta,
                                         SiteProduto)
                                            VALUES
                                        (@NomeProduto,
                                         @PrecoAntigo,
                                         @PrecoAtual,
                                         @DataConsulta,
                                         @SiteProduto);";

            using (var cmd = new SqlCommand(comandoSql, _conexao))
            {
                cmd.Parameters.AddWithValue("@NomeProduto", register.nomeProduto);
                cmd.Parameters.AddWithValue("@PrecoAntigo", register.precoAntigo);
                cmd.Parameters.AddWithValue("@PrecoAtual", register.precoAtual);
                cmd.Parameters.AddWithValue("@DataConsulta", register.dataConsulta);
                cmd.Parameters.AddWithValue("@SiteProduto", register.siteProduto);

                cmd.ExecuteNonQuery();
            }
        }
    }
}


