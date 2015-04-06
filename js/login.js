/*global $:false */
var MK = MK || {};
MK.vm = {};

MK.vm.Login = function () {
    var email = ko.observable(""),
        password = ko.observable(""),
        recover = ko.observable(false);

    function login() {
        Parse.User.logIn(email(), password(), {
          success: function(user) {
            document.location.href = "/index.html";
          },
          error: function(user, error) {
            errorAlert("Error: " + error.code + " " + error.message);
          }
        });
    }

    function resetPassword() {
        resetAlerts();
        password("");
        var query = new Parse.Query(Parse.User);

        query.equalTo("username", email());
        query.limit(1);
        query.find({
          success: function(results) {
            var user = results[0];
            user.set("password", "potatis");

            user.save(null, {
              success: function(user) {
                recover(false);
                infoAlert("Ditt lösenord är återställt till: <strong>potatis</strong>")
              },
              error: function(user, error) {
                errorAlert("Error: " + error.code + " " + error.message);
              }
            });
          },
          error: function(error) {
            errorAlert("Error: " + error.code + " " + error.message);
          }
        });
    }

    function errorAlert(message) {
        $('#alerts').replaceWith('<div id="alerts"><hr><div class="alert alert-danger" role="alert">' + message + '</div></div>');
    }

    function infoAlert(message) {
        $('#alerts').replaceWith(
            '<div id="alerts"><hr><div class="alert alert-info" role="alert">' + message + '</div></div>'
            );
    }

    function resetAlerts() {
        $('#alerts').replaceWith('<div id="alerts"></div>');
    }

    return {
        email: email,
        password: password,
        login: login,
        recover: recover,
        resetPassword: resetPassword
    }
}