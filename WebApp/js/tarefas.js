exibirElementosTarefas();
function exibirElementosEmpresaTarefas() {
    if (nivelAcesso == '1') {
        $("#cardCadastroTarefa").show();
    }
}

$(document).ready(function () {
    ListarTarefas();
});

var tabelaTarefas;
var urlBaseApi = "https://localhost:44382";

function LimparCorpoTabelaTarefas() {
    var componenteSelecionado = $('#tabelaTarefas tbody');
    componenteSelecionado.html('');
}

function ListarTarefas() {
    var rotaApi = '/tarefa';

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
        var botaoAlterar = '<button class="btn btn-primary btn-sm me-2" onclick="Alterar(' + linha.tarefaId + ')">Alterar</button>';
        var botaoExcluir = '<button class="btn btn-danger btn-sm" onclick="Excluir(' + linha.tarefaId + ')">Excluir</button>';

        htmlTabela = htmlTabela + `<tr><th>${linha.tarefaId}</th><td>${FormatarData(linha.horarioInicio)}</td><td>${FormatarData(linha.horarioFim)}</td><td>${linha.resumoTarefa}</td><td>${linha.descricaoTarefa}</td><td>${(linha.Cnpj)}</td><td>${linha.tipoTarefaId}</td><td>${botaoAlterar + botaoExcluir}</td></tr>`;
    });

    $('#tabelaTarefas tbody').html(htmlTabela);
    if (tabelaTarefas == undefined) {
        tabelaTarefas = $('#tabelaTarefas').DataTable({
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.13.1/i18n/pt-BR.json'
            }
        });
    }
}

function ObterValoresFormulario() {
    var tarefaId = $("#inputTarefaId").val();
    var horarioInicio = $("#inputHorarioInicio").val();
    var horarioFim = $("#inputHorarioFim").val();
    var resumoTarefa = $("#inputResumoTarefa").val();
    var descricaoTarefa = $("#inputDescricaoTarefa").val();
    var Cnpj = $("#inputCnpj").val();
    var tipoTarefaId = $("#inputTipoTarefaId").val();

    var objeto = {

        tarefaId : tarefaId,
        horarioInicio : horarioInicio,
        horarioFim : horarioFim,
        resumoTarefa : resumoTarefa,
        descricaoTarefa : descricaoTarefa,
        Cnpj: removeCnpjMask(Cnpj),
        tipoTarefaId : tipoTarefaId,
    };

    return objeto;
}

function EnviarFormularioParaApi() {
    var rotaApi = '/tarefa';

    var objeto = ObterValoresFormulario();
    var json = JSON.stringify(objeto);

    var isEdicao = $("#inputTarefaId").is(":disabled");
    if (isEdicao) {
        $.ajax({
            url: urlBaseApi + rotaApi,
            method: 'PUT',
            data: json,
            contentType: 'application/json'
        }).done(function () {
            VoltarEstadoInsercaoFormulario();
            ListarTarefas();
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Tarefa alterada com sucesso.',
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
            ListarTarefas();
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Tarefa adicionado com sucesso.',
                showConfirmButton: false,
                timer: 1500
            });
        });
    }
}

function LimparFormulario() {
    $('#formTarefas').trigger("reset");
}

function SubmeterFormulario() {
    var isValido = $('#formTarefas').parsley().validate();
    if (isValido) {
        EnviarFormularioParaApi();
    }
}
function LimparFormulario() {
    $('#formTarefas').trigger("reset");
}

function SubmeterFormulario() {
    var isValido = $('#formTarefas').parsley().validate();
    if (isValido) {
        EnviarFormularioParaApi();
    }
}

function Excluir(tarefaId) {
    Swal.fire({
        title: 'Você quer excluir essa tarefa?',
        showDenyButton: true,
        confirmButtonText: 'Sim',
        denyButtonText: `Não`,
    }).then((result) => {
        if (result.isConfirmed) {
            EnviarExclusao(tarefaId);
        } else if (result.isDenied) {
            Swal.fire('Nada foi alterado.', '', 'info')
        }
    });
}

function EnviarExclusao(tarefaId) {
    var rotaApi = '/tarefa/' + tarefaId;

    $.ajax({
        url: urlBaseApi + rotaApi,
        method: 'DELETE',
    }).done(function () {
        ListarTarefas();
        Swal.fire('Tarefa excluída com sucesso.', '', 'success');
    });
}

function Alterar(tarefaId) {
    var rotaApi = '/tarefa/' + tarefaId;

    $.ajax({
        url: urlBaseApi + rotaApi,
        method: 'GET',
        dataType: "json"
    }).done(function (resultado) {
        $("#inputTarefaId").val(resultado.tarefaId);
        $("#inputHorarioInicio").val(FormatarDataAmericana(resultado.horarioInicio));
        $("#inputHorarioFim").val(FormatarDataAmericana(resultado.horarioFim));
        $("#inputResumoTarefa").val(resultado.resumoTarefa);
        $("#inputDescricaoTarefa").val(resultado.descricaoTarefa);
        $("#inputCnpj").val(resultado.Cnpj);
        $("#inputTipoTarefaId").val(resultado.tipoTarefaId);

        $("#inputTarefaId").prop("disabled", true);
    });
}

function BotaoCancelar() {
    var isEdicao = $("#inputTarefaId").is(":disabled");

    if (isEdicao) {
        VoltarEstadoInsercaoFormulario();
    } else {
        LimparFormulario();
    }
}

function VoltarEstadoInsercaoFormulario() {
    LimparFormulario();
    $("#inputTarefaId").prop("disabled", false);
}

function BaixarRelatorio() {
    fetch('https://localhost:44382/relatorio')
        .then(resp => resp.blob())
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            // the filename you want
            a.download = 'relatorio.csv';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        });
}