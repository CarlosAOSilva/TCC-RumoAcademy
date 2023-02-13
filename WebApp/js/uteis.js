$(document).ready(function () {
    $('.cnpj').mask('00.000.000/0000-00');

    $.ajaxSetup({
        headers: { 'Authorization': 'Bearer' + localStorage.getItem('bearer') },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 400) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Oops...',
                    text: jqXHR.responseText
                });
            } else if (jqXHR.status == 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Os nossos servidores ou sua internet estão indisponíveis no momento, tente novamente mais tarde.'
                });
            }
            else if (jqXHR.status == 401) {
                Swal.fire({
                    icon: 'info',
                    title: 'Oops...',
                    text: 'As suas credenciais expiraram, faça login novamente.'
                }).then((result) => {
                    window.location.href = "login.html";
                });
            }
            else if (jqXHR.status == 403) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Acesso Negado',
                    text: 'Você não tem permissão para acessar este recurso.'
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: jqXHR.responseText
                });
            }
        }
    });
});

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

function removerCnpjMask(cnpj) {
    return cnpj.replace(/\D/g, '');
}

var nivelAcesso = localStorage.getItem('nivelAcesso');

