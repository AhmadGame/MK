
var MK = MK || {};
MK.util = MK.util || {};

MK.util.getParameterByName = function(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(document.location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

MK.server = (function($) {
    function getJson(url, onSuccess, onFailure) {
        $.ajax({
            dataType: 'json',
            url: url,
            success: function(data, textStatus, jqXhr) {
                if (data.redirectUrl) {
                    document.location.href = data.redirectUrl;
                } else if (_.isFunction(onSuccess)) {
                    onSuccess(data, textStatus, jqXhr);
                }
            },
            error: function(jqXhr, textStatus, errorThrown) {
                if (_.isFunction(onFailure)) {
                    onFailure(jqXhr, textStatus, errorThrown);
                }
            }
        });
    }

    function sendHttp(method, url, data, onSuccess, onFailure) {
        $.ajax({
            url: url,
            type: method,
            data: JSON.stringify(data),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: onSuccess,
            error: function(jqXhr, textStatus, errorThrown) {
                if (jqXhr.status === 202) {
                    if (_.isFunction(onSuccess)) {
                        onSuccess(null, textStatus, jqXhr);
                    }
                } else {
                    if (_.isFunction(onFailure)) {
                        onFailure(jqXhr, textStatus, errorThrown);
                    }
                }
            }
        });
    }

    function postJson(url, data, onSuccess, onFailure) {
        sendHttp('POST', url, data, onSuccess, onFailure);
    }

    function putJson(url, data, onSuccess, onFailure) {
        sendHttp('PUT', url, data, onSuccess, onFailure);
    }

    function deleteJson(url, data, onSuccess, onFailure) {
        sendHttp('DELETE', url, data, onSuccess, onFailure);
    }

    return {
        getJson: getJson,
        postJson: postJson,
        putJson: putJson,
        deleteJson: deleteJson
    };
}(jQuery));