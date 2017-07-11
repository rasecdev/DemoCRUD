function configurarControles() {

    var traducao = {
        infos: "Exibindo {{ctx.start}} até {{ctx.end}} de {{ctx.total}} registros",
        loading: "Carregando, isso pode demorar alguns segundos...",
        noResults: "Não há dados  para exibir",
        refresh: "Atualizar",
        search: "Pesquisar"
    };

    //Configuração do BootGrid
    //Variável com as informações do BootGrid
    var grid = $("#gridDados").bootgrid({

        ajax: true,
        url: urlListas,
        labels: traducao,
        searchSettings: {
            characters: 4
        },
        formatters: {
            "acoes": function (column, row) {
                //data-acao atribui uma informação personalizada ao elemento.
                //data-row-id para recupeara o id da linha que é o id do Livro, será usado nos seus respectivos
                //ActionResult.
                return "<a href='#' class='btn btn-info' data-acao='Details' data-row-id='" + row.Id + "'>" +
                    "<span class='glyphicon glyphicon-list'></span></a>" +
                    "<a href='#' class='btn btn-warning' data-acao='Edit' data-row-id='" + row.Id + "'>" +
                    "<span class='glyphicon glyphicon-edit'></span></a>" +
                    "<a href='#' class='btn btn-danger' data-acao='Delete' data-row-id='" + row.Id + "'>" +
                    "<span class='glyphicon glyphicon-trash'></span></a>";
            }
        }
    });

    //Referência ao Evento do elemento grid (loaded.rs.jquery.bootgrid) site do bootgrid
    //Configurar a chamada para o tratamento do click nos botões Details, Edit e Delete.
    //each ver site do bootgrid em API Document.
    grid.on("loaded.rs.jquery.bootgrid", function () {

        grid.find("a.btn").each(function (index, elemento) {

            var botaoDeAcao = $(this);

            var acao = botaoDeAcao.data("acao");

            var idEntidade = botaoDeAcao.data("row-id");

            $(elemento).on("click", function () {
                abrirModal(acao, idEntidade);
            });

        });
    });

    //Chamada para função abrirModal apartir do click em uma tag <a> com a clase btn
    $("a.btn").click(function () {
        var acao = $(this).data("action");
        abrirModal(acao);
    });
}

function abrirModal(acao, parametro) {

    var url = "/ctrl/acao/parametro";

    url = url.replace("ctrl", controller);
    url = url.replace("acao", acao);

    if (parametro != null) {
        url = url.replace("parametro", parametro);
    } else {
        url = url.replace("parametro", "");
    }

    $("#conteudoModal").load(url, function () {

        $("#minhaModal").modal('show');

    });
}