/*global $:false */
var MK = MK || {};
MK.vm = {};

MK.vm.Register = function () {
    var name = ko.observable(""),
        email = ko.observable(""),
        password = ko.observable(""),
        success = false;

    function register() {
        var user = new Parse.User();
        user.set("username", email());
        user.set("password", password());
        user.set("email", email());
        user.set("name", name());

        user.signUp(null, {
          success: function(user) {
            document.location.href = "/login.html"
          },
          error: function(user, error) {
            addAlert("Error: " + error.code + " " + error.message);
          }
        });
    }

    function addAlert(message) {
        $('#alerts').replaceWith('<div id="alerts"><hr><div class="alert alert-danger" role="alert">' + message + '</div></div>');
    }

    return {
        name: name,
        email: email,
        password: password,
        register: register
    }
}