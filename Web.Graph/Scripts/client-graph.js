var syntaxHighlight = function (json) {
    if (typeof json != 'string') {
        json = JSON.stringify(json, undefined, 2);
    }
    json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
        var cls = 'number';
        if (/^"/.test(match)) {
            if (/:$/.test(match)) {
                cls = 'key';
            } else {
                cls = 'string';
            }
        } else if (/true|false/.test(match)) {
            cls = 'boolean';
        } else if (/null/.test(match)) {
            cls = 'null';
        }
        return '<span class="' + cls + '">' + match + '</span>';
    });
}

var setOut = function (jObject) {
    window.ace.edit("editorO").setValue(JSON.stringify(jObject, undefined, 2));

    console.log($('#Result').is(':visible'));
    $('html, body').animate({
            scrollTop: $("#Result").offset().top - 90
        },
        800);
}

var send = function () {
    var $frm = $(this);
    window.ace.edit("editorO").setValue("Ejecutando...");
    $.ajax({
        url : $frm.attr('action'),
        method: $frm.attr('method'),
        data: "=" + window.ace.edit("editorQ").getValue(),
        dataType : 'json',
        error: function () {
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
            "telefono", "fax", "comercio_exterior", "principal", "secundario2", "rus", "buen_contribuyente", "retencion",
            "percepcion_vinterna", "percepcion_cliquido"];
        callback(null, wordList.map(function (word) {
            return {
                caption: word,
                value: word,
                meta: "static"
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