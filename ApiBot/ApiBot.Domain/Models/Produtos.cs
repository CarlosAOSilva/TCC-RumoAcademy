using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBot.Domain.Models
{
    public class Produtos
    {
        public string? nomeProduto {get; set;}   

        public decimal precoAntigo {get; set;}

        public decimal precoAtual { get; set;}

        public DateTime dataConsulta { get; set; } = DateTime.Now;

        public string? siteProduto { get; set; }
    }
}
