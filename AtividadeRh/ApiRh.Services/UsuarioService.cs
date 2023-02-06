using System.Text.RegularExpressions;
using System.Text;
using ApiTarefas.Repositories;
using ApiTarefas.Domain.Models;

namespace ApiTarefas.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepositories _repositorio;
        public UsuarioService(UsuarioRepositories repositorio)
        {
            _repositorio = repositorio;
        }

        public Usuario? ObterUsuarioPorCredenciais(string email, string senha, bool isDescriptografado = true)
        {
            try
            {
                _repositorio.AbrirConexao();

                if (isDescriptografado)
                    senha = CriptografarSha512(senha);

                return _repositorio.ObterUsuarioPorCredenciais(email, senha);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }

        private static string CriptografarSha512(string texto)
        {
            var bytes = Encoding.UTF8.GetBytes(texto);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString().ToLower();
            }
        }
    }
}
