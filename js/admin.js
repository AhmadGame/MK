/*global $:false */
var MK = MK || {};
MK.vm = {};

MK.vm.Admin = function () {
    //****************************************
    // login
    //****************************************
    var loggedIn = ko.observable(false);
    var adminPassword = ko.observable("");

    function login() {
        Parse.User.logIn("admin", adminPassword(), {
          success: function(user) {
            loggedIn(true);
            initViewQuestions();
          },
          error: function(user, error) {
            errorAlert("Error: " + error.code + " " + error.message);
          }
        });
    }

    function errorAlert(message) {
        $('#alerts').replaceWith('<div id="alerts"><hr><div class="alert alert-danger" role="alert">' + message + '</div></div>');
    }

    //****************************************
    // addUser
    //****************************************
    var addUser = ko.observable(false);
    var userName = ko.observable(""),
        userEmail = ko.observable(""),
        userPassword = ko.observable("");

    function initAddUser() {
        
        addUser(true); 
        viewQuestions(false); 
        viewUsers(false); 
        initPopover();
    }

    function register() {
        
        var user = new Parse.User();
        user.set("username", userEmail());
        user.set("password", userPassword());
        user.set("email", userEmail());
        user.set("name", userName());

        user.signUp(null, {
          success: function(user) {
            alert("Användaren har sparats!");

            userEmail('');
            userPassword('');
            userEmail('');
            userName('');
          },
          error: function(user, error) {
            errorAlert("Error: " + error.code + " " + error.message);
          }
        });
    }

    function initPopover() {
        
        //minimum 8 characters
        var bad = /(?=.{8,}).*/;
        //Alpha Numeric plus minimum 8
        var good = /^(?=\S*?[a-z])(?=\S*?[0-9])\S{8,}$/;
        //Must contain at least one upper case letter, one lower case letter and (one number OR one special char).
        var better = /^(?=\S*?[A-Z])(?=\S*?[a-z])((?=\S*?[0-9])|(?=\S*?[^\w\*]))\S{8,}$/;
        //Must contain at least one upper case letter, one lower case letter and (one number AND one special char).
        var best = /^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])(?=\S*?[^\w\*])\S{8,}$/;

        $('#password').on('keyup', function () {
            var password = $(this);
            var pass = password.val();
            var passLabel = $('[for="password"]');
            var stength = 'Weak';
            var pclass = 'danger';
            if (best.test(pass) == true) {
                stength = 'Very Strong';
                pclass = 'success';
            } else if (better.test(pass) == true) {
                stength = 'Strong';
                pclass = 'warning';
            } else if (good.test(pass) == true) {
                stength = 'Almost Strong';
                pclass = 'warning';
            } else if (bad.test(pass) == true) {
                stength = 'Weak';
            } else {
                stength = 'Very Weak';
            }

            var popover = password.attr('data-content', stength).data('bs.popover');
            popover.setContent();
            popover.$tip.addClass(popover.options.placement).removeClass('danger success info warning primary').addClass(pclass);

        });

        $('input[data-toggle="popover"]').popover({
            placement: 'top',
            trigger: 'focus'
        });
    }

    //****************************************
    // viewUsers
    //****************************************
    var viewUsers = ko.observable(false);
    var users = ko.observable([]);
    var userCount = ko.observable(0);
    var usersLoaded = ko.observable(0);
    var page = 1;

    function initViewUsers() {
        
        addUser(false); 
        viewQuestions(false); 
        viewUsers(true);
        
        page = 1;
        getUsers(0);
        countUsers();
    }

    function countUsers () {
        var query = new Parse.Query(Parse.User);
        query.count({
            success: function(count) {
                userCount(count);
            },
            error: function(error) {
                alert("Error: " + error.code + " " + error.message);
            }
        });
    }

    function getUsers(skip) {

        var query = new Parse.Query(Parse.User);
        query.limit(20);
        query.skip(skip);
        query.find({
          success: function(results) {
            users(_.map(results, function (parseObject) {
                var user = new MK.vm.user();
                user.init(parseObject);
                return user;
            }));
            usersLoaded(users().length * page);
          },
          error: function(error) {
            alert("Error: " + error.code + " " + error.message);
          }
        });
    }

    function nextUsers () {
        if (userCount() > users().length) {
            getUsers(users().length * page);
            page++;
        };
    }

    function prevUsers () {
        if (page > 1) {
            page--;
            getUsers(users().length * page);
        }
    }

    //****************************************
    // viewQuestions
    //****************************************
    var viewQuestions = ko.observable(false);
    var questions = ko.observable([]);
    var questionCount = ko.observable(0);
    var questionsLoaded = ko.observable(0);
    
    function initViewQuestions() {
        
        addUser(false); 
        viewQuestions(true); 
        viewUsers(false);

        page = 1;
        getQuestions(0);
        countQuestions();
    }

    function getQuestions(skip) {

        var Question = Parse.Object.extend("question");
        var query = new Parse.Query(Question);
        query.limit(20);
        query.skip(skip);
        query.find({
          success: function(results) {
            questions(_.map(results, function (parseObject) {
                var q = new MK.vm.question();
                q.init(parseObject);
                return q;
            }));
            questionsLoaded(questions().length * page);
          },
          error: function(error) {
            alert("Error: " + error.code + " " + error.message);
          }
        });
    }

    function countQuestions () {
        var Questions = Parse.Object.extend("question");
        var query = new Parse.Query(Questions);
        query.count({
            success: function(count) {
                questionCount(count);
            },
            error: function(error) {
                alert("Error: " + error.code + " " + error.message);
            }
        });
    }

    function nextQuestions () {
        if (questionCount() > questions().length) {
            getQuestions(questions().length * page);
            page++;
        };
    }

    function prevQuestions () {
        if (page > 0) {
            page--;
            getQuestions(questions().length * page);
        }
    }

    return {
        loggedIn: loggedIn,
        adminPassword: adminPassword,
        login: login,

        addUser: addUser,
        initAddUser: initAddUser,
        userName: userName,
        userEmail: userEmail,
        userPassword: userPassword,
        register: register,
        initPopover: initPopover,

        viewQuestions: viewQuestions,
        initViewQuestions: initViewQuestions,
        questions: questions,
        nextQuestions: nextQuestions,
        prevQuestions: prevQuestions,
        questionCount: questionCount,
        questionsLoaded: questionsLoaded,

        viewUsers: viewUsers,
        initViewUsers: initViewUsers,
        users: users,
        nextUsers: nextUsers,
        prevUsers: prevUsers,
        userCount: userCount,
        usersLoaded: usersLoaded
    }
}

MK.vm.user = function() {
    
    var self = this;
    var name, email;
    
    function init(parseObject) {
        self.name = parseObject.get("name");
        self.email = parseObject.get("username");
    }

    function deleteUser () {
        var query = new Parse.Query(Parse.User);
        query.equalTo("name", self.name);
        query.equalTo("email", self.email);
        query.first({
          success: function(myObj) {
               myObj.destroy({
                success: function(myObject) {
                    alert("Användaren har tagits bort!");
                },
                error: function(myObject, error) {
                    alert("Error: " + error.code + " " + error.message);
                }
            });
          },
          error: function(myObj, error) {
            alert("Error: " + error.code + " " + error.message);
          }
        });
    }

    this.name = name;
    this.email = email;
    this.init = init;
    this.deleteUser = deleteUser;
}
