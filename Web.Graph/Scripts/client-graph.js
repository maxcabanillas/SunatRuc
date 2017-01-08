var setStateButton = function (state) {
    $('#btnExecute').prop('disabled', state);
    var ico = $('#icoEx');
    if (state === true)
        ico.removeClass("fa-play").addClass("fa-cog fa-spin");
    else
        ico.removeClass("fa-cog fa-spin").addClass("fa-play");
}

var setOut = function (jObject) {
    window.ace.edit("editorO").setValue(JSON.stringify(jObject, undefined, 2));
    setStateButton(false);
    console.log($('#Result').is(':visible'));
    $('html, body').animate({
            scrollTop: $("#Result").offset().top - 90
        },
        800);
}

var send = function () {
    var $frm = $(this);
    window.ace.edit("editorO").setValue("Ejecutando...");
    setStateButton(true);
    $.ajax({
        url : $frm.attr('action'),
        method: $frm.attr('method'),
        data: "=" + window.ace.edit("editorQ").getValue(),
        dataType : 'json',
        error: function () {
            setStateButton(false);
            alert("Invalid Request");
        }
    }).done(function( msg ) {
        setOut(msg);
    });
    return false;
};
$("#frmQuery").submit(send);
var editor = ace.edit("editorQ");
editor.getSession().setUseWorker(false);
editor.getSession().setMode("ace/mode/json");
editor.setTheme("ace/theme/xcode");
editor.renderer.setShowGutter(false);
editor.setShowPrintMargin(false);
editor.setOptions({
    enableBasicAutocompletion: true,
    enableSnippets: false,
    enableLiveAutocompletion: true
});
var staticWordCompleter = {
    getCompletions: function (editor, session, pos, prefix, callback) {
        var wordList = ["query", "empresa", "ruc", "nombre", "tipo_contribuyente", "profesion", "nombre_comercial", "condicion_contribuyente",
            "estado_contribuyente", "fecha_inscripcion", "fecha_inicio", "departamento", "provincia", "distrito", "direccion",
            "telefono", "fax", "comercio_exterior", "principal","secundario1", "secundario2", "rus", "buen_contribuyente", "retencion",
            "percepcion_vinterna", "percepcion_cliquido", "persona", "primer_nombre", "segundo_nombre", "apellido_paterno", "apellido_materno"];
        callback(null, wordList.map(function (word) {
            return {
                caption: word,
                value: word == "persona" ? "persona (dni:\"\"){\r\n        primer_nombre\r\n    }" : word,
                meta: "GraphQL"
            };
        }));
    }
}
editor.completers = [staticWordCompleter];

var editor2 = ace.edit("editorO");
editor2.getSession().setUseWorker(false);
editor2.getSession().setMode("ace/mode/json");
editor2.setTheme("ace/theme/monokai");
editor2.setReadOnly(true);
editor2.renderer.setShowGutter(false);
editor2.setShowPrintMargin(false);
//Style editors
var h = $(window).height() * 3/5;
$('#editorQ').attr("style", "height: " + h + "px");
$('#editorO').attr("style", "height: " + h + "px");//40rem