$(document).ready(function () {
    ListarProdutos();
})

var tabelaProduto;
var urlBaseApi = "https://localhost:44377";

function LimparCorpoTabelaProdutos() {
    var componenteSelecionado = $('#tabelaProduto tbody');
    componenteSelecionado.html('');
}

function ListarProdutos() {
    var rotaApi = '/produto';

    $.ajax({
        url: urlBaseApi + rotaApi,
        method: 'GET',
        dataType: "json"
    }).done(function (resultado) {
        ConstruirTabela(resultado);
    }).fail(function (err, errr, errrr) {

    });
}

function ConstruirTabela(linhas) {

    var htmlTabela = '';

    $(linhas).each(function (index, linha) {
       
        var moedaIsOk = "";
        if (linha.precoAntigo == 0) {
            moedaIsOk = "text-primary";
        } else if (linha.preco > linha.precoAntigo) {
            moedaIsOk = "text-danger";
        } else if (linha.preco < linha.precoAntigo) {
            moedaIsOk = "text-success";
        }

        htmlTabela += `<tr><th id="nomeP">${linha.nomeProduto}</a></th>
                        <td id="moeda01" class="${moedaIsOk}">${FormatarMoeda(linha.precoAtual)}</td>
                        <td id="moeda02">${FormatarMoeda(linha.precoAntigo == 0) ? "Não há registro de preço antigo" : linha.precoAntigo}</td>
                        <td id="data01">${FormatarData(linha.dataConsulta)}</td></tr>`
    });

    $('#tabelaProduto tbody').html(htmlTabela);
    if (tabelaProduto == undefined) {
        tabelaProduto = $('#tabelaProduto').DataTable({
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.13.2/i18n/pt-BR.json'
            }
        });
    }
}

function AtualizarProdutos() {
    var rotaApi = '/produto';

    $.ajax({
        url: urlBaseApi + rotaApi,
        method: 'POST',
        
    }).done(function () {
        ListarProdutos();
        Swal.fire({
            position: 'center',
            icon: 'success',
            title: 'Produtos Atualizados Com Sucesso!!!',
            showConfirmButton: false,
            timer: 1500
        });
    }).fail(function (err, errr, errrr) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Erro!!! Aconteceu algo Inesperado!!!',
        });

    });
}
function FormatarData(dataString) {
    var dataTeste = new Date(dataString);
    var ano = dataTeste.getFullYear();
    var mes = String(dataTeste.getMonth() + 1).length == 1 ? '0' + String(dataTeste.getMonth() + 1) : String(dataTeste.getMonth() + 1);
    var dia = String(dataTeste.getDate()).length == 1 ? '0' + String(dataTeste.getDate()) : String(dataTeste.getDate());

    return dia + "/" + mes + "/" + ano;
}

function FormatarDataAmericana(dataString) {
    var dataTeste = new Date(dataString);
    var ano = dataTeste.getFullYear();
    var mes = String(dataTeste.getMonth() + 1).length == 1 ? '0' + String(dataTeste.getMonth() + 1) : String(dataTeste.getMonth() + 1);
    var dia = String(dataTeste.getDate()).length == 1 ? '0' + String(dataTeste.getDate()) : String(dataTeste.getDate());

    return ano + "-" + mes + "-" + dia;
}
function FormatarMoeda(value) {
    return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }