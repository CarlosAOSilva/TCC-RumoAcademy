using ApiBot.Domain.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutoBot
{
    public class Bot
    {
        private readonly string _urlBase;

        public Bot(string urlBase)
        {
            _urlBase = urlBase;
        }
        public List<Produtos> Obter()
        {
            var client = new HttpClient();
            var result = client.GetAsync("https://www.bazardebagda.com.br/?view=ecom/itens&tcg=1&txt_edicao=479782&txt_estoque=0&itens_total=297&txt_limit=30&txt_order=6&page=1").Result;
            if (!result.IsSuccessStatusCode)
                throw new Exception("Falha ao conectar com o site");

            Utf8EncodingProvider.Register();
            var html = result.Content.ReadAsStringAsync().Result;

            var totalPagina = 10;
            //Ao todo são mais de 300, mas para ficar um programa mais simples, coloquei apenas 10

            var paginas = Enumerable.Range(1, totalPagina);

            var listaProdutos = new List<Produtos>();

            foreach (var pagina in paginas)
            {
                result = client.GetAsync(_urlBase + "/?view=ecom/itens&tcg=1&txt_edicao=479782&txt_estoque=0&itens_total=297&txt_limit=30&txt_order=6&page=" + pagina).Result;
                html = result.Content.ReadAsStringAsync().Result;

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var produtos = doc.DocumentNode.SelectNodes("//div[contains(@class, 'card-item')]");

                if (produtos is null)
                    throw new Exception("Erro ao coletar elementos");

                foreach (var produto in produtos)
                {
                    var elementoPreco = produto.SelectNodes(".//span[contains(@class, 'align-price')]");
                    if (elementoPreco is null)
                        continue;

                    var preco = Convert.ToDecimal(elementoPreco[0].InnerText.Replace("R$ ", "").Replace(" ", ""));
                    var elementoA = produto.Descendants("a").First();
                    var linkProduto = elementoA.Attributes["href"].Value;
                    var linkCompleto = linkProduto.Replace("./", "/");
                    var titulo = produto.SelectNodes(".//div[contains(@class, 'title')]").First().InnerText.Replace("\"", "");

                    listaProdutos.Add(new Produtos
                    {
                        nomeProduto = titulo,
                        siteProduto = linkCompleto,
                        precoAtual = preco
                    });
                }
            }
            return listaProdutos;
        }
        public class Utf8EncodingProvider : EncodingProvider
        {
            public override Encoding GetEncoding(string name)
            {
                return name == "utf8" ? Encoding.UTF8 : null;
            }

            public override Encoding GetEncoding(int codepage)
            {
                return null;
            }

            public static void Register()
            {
                Encoding.RegisterProvider(new Utf8EncodingProvider());
            }
        }
    }
}

