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
    $('#log').html(syntaxHighlight(JSON.stringify(jObject, undefined, 2)));

    console.log($('#Result').is(':visible'));
    $('html, body').animate({
            scrollTop: $("#Result").offset().top - 90
        },
        800);
}

var send = function () {
    var $frm = $(this);
    $('#log').text("Ejecutando...");
    $.ajax({
        url : $frm.attr('action'),
        method: $frm.attr('method'),
        data: "=" + this.query.value,
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