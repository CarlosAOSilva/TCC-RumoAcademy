exibirElementosEmpresa();
function exibirElementosEmpresa() {
    if (nivelAcesso == '1') {
        $("#cardCadastroEmpresas").show();
    }
}

$(document).ready(function () {
    ListarEmpresas();
});

var tabelaEmpresa;
var urlBaseApi = "https://localhost:44382";

function LimparCorpoTabelaEmpresas() {
    var componenteSelecionado = $('#tabelaEmpresas tbody');
    componenteSelecionado.html('');
}

function ListarEmpresas() {
    var rotaApi = '/empresas';

    $.ajax({
        url: urlBaseApi + rotaApi,
        method: 'GET',
        dataType: "json"
    }).done(function (resultado) {
        ConstruirTabela(resultado);
    });
}

function ConstruirTabela(linhas) {

    var htmlTabela = '';

    $(linhas).each(function (index, linha) {
        var botaoAlterar = '<button class="btn btn-primary btn-sm me-2" onclick="Alterar(' + linha.Cnpj + ')">Alterar</button>';
        var botaoExcluir = '<button class="btn btn-danger btn-sm" onclick="Excluir(' + linha.Cnpj + ')">Excluir</button>';

        htmlTabela = htmlTabela + `<tr><th>${linha.Cnpj}</th><td>${linha.razaoSocial}</td><td>${FormatarData(linha.dataCadastro)}</td><td>${botaoAlterar + botaoExcluir}</td></tr>`;
    });

    $('#tabelaEmpresas tbody').html(htmlTabela);
    if (tabelaEmpresa == undefined) {
        tabelaEmpresa == $('#tabelaEmpresas').DataTable({
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.13.1/i18n/pt-BR.json'
            }
        });
    }
}

function ObterValoresFormulario() {
    var Cnpj = $("#inputCnpj").val();
    var razaoSocial = $("#inputRazaoSocial").val();
    var dataCadastro = $("#inputDataCadastro").val();

    var objeto = {
        Cnpj: (Cnpj),
        razaoSocial: razaoSocial,
        dataCadastro: dataCadastro,
    };

    return objeto;
}

function EnviarFormularioParaApi() {
    var rotaApi = '/empresas';

    var objeto = ObterValoresFormulario();
    var json = JSON.stringify(objeto);

    var isEdicao = $("#inputCnpj").is(":disabled");

    if (isEdicao) {
        $.ajax({
            url: urlBaseApi + rotaApi,
            method: 'PUT',
            data: json,
            contentType: 'application/json'
        }).done(function () {
            VoltarEstadoInsercaoFormulario();
            ListarEmpresas();
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Empresa alterada com sucesso.',
                showConfirmButton: false,
                timer: 1500
            });
        });
    } else {
        $.ajax({
            url: urlBaseApi + rotaApi,
            method: 'POST',
            data: json,
            contentType: 'application/json'
        }).done(function () {
            LimparFormulario();
            ListarEmpresas();
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Empresa adicionado com sucesso.',
                showConfirmButton: false,
                timer: 1500
            });
        });
    }
}

function LimparFormulario() {
    $('#formEmpresas').trigger("reset");
}

function SubmeterFormulario() {
    var isValido = $('#formEmpresas').parsley().validate();
    if (isValido) {
        EnviarFormularioParaApi();
    }
}

function Excluir(cnpj) {
    Swal.fire({
        title: 'Você quer excluir esse cliente?',
        showDenyButton: true,
        confirmButtonText: 'Sim',
        denyButtonText: `Não`,
    }).then((result) => {
        if (result.isConfirmed) {
            EnviarExclusao(cnpj);
        } else if (result.isDenied) {
            Swal.fire('Nada foi alterado.', '', 'info')
        }
    });
}

function EnviarExclusao(cnpj) {
    var rotaApi = '/empresas/' + cnpj;

    $.ajax({
        url: urlBaseApi + rotaApi,
        method: 'DELETE',
    }).done(function () {
        ListarEmpresas();
        Swal.fire('Cliente excluido com sucesso.', '', 'success');
    });
}

function Alterar(cnpj) {
    var rotaApi = '/empresas/' + cnpj;

    $.ajax({
        url: urlBaseApi + rotaApi,
        method: 'GET',
        dataType: "json"
    }).done(function (resultado) {
        $("#inputCnpj").val(resultado.Cnpj);
        $("#inputRazaoSocial").val(resultado.razaoSocial);
        $("#inputDataCadastro").val(FormatarDataAmericana(resultado.dataCadastro));

        $("#inputCnpj").prop("disabled", true);
    });
}

function BotaoCancelar() {
    var isEdicao = $("#inputCnpj").is(":disabled");

    if (isEdicao) {
        VoltarEstadoInsercaoFormulario();
    } else {
        LimparFormulario();
    }
}

function VoltarEstadoInsercaoFormulario() {
    LimparFormulario();
    $("#inputCnpj").prop("disabled", false);
}
