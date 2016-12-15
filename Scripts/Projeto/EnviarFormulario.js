//Modificação para utilizar a PartialView com Jason

//Referência para o botão do Create. (Botão acima!)
var btnAcao = $("input[type='button']");

//Referência do formulário para entregar ao usuário a validação do jquery Validation.
var formulario = $("#formCrud");

btnAcao.on("click", submeter);

function submeter() {

    var url = formulario.prop("action");

    var metodo = formulario.prop("method");

    var dadosFormulario = formulario.serialize();

    $.ajax({
        url: url,
        type: metodo,
        data: dadosFormulario,
        success: tratarRetorno
    });
}

//Função para enviar os dados para o servidor.
function tratarRetorno(resultadoServidor) {

    if (resultadoServidor.resultado) {

        toastr['success'](resultadoServidor.mensagem);

        $('#minhaModal').modal('hide');

        //Para mostrar o novo livro no index.
        $('#gridDados').bootgrid('reload');
    }
    else {

        toastr['error'](resultadoServidor.mensagem);
    }
}