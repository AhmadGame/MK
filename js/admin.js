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
    // addQuestion
    //****************************************
    var addQuestion = ko.observable(false);
    var title = ko.observable(""),
        answer1 = ko.observable(""),
        answer2 = ko.observable(""),
        answer3 = ko.observable(""),
        answer4 = ko.observable(""),
        correct = ko.observable(0),
        explain1 = ko.observable(""),
        explain2 = ko.observable(""),
        explain3 = ko.observable(""),
        explain4 = ko.observable(""),
        image1 = ko.observable(""),
        image2 = ko.observable(""),
        image3 = ko.observable(""),
        image4 = ko.observable(""),
        page_reference = ko.observable("");

    function initAddQuestion() {

        addUser(false); 
        addQuestion(true); 
        viewQuestions(false); 
        viewUsers(false); 
    }

    function save() {

        var Question = Parse.Object.extend("question");
        var question = new Question();
         
        question.set("title", title());
        question.set("answer1", answer1());
        question.set("answer2", answer2());
        question.set("answer3", answer3());
        question.set("answer4", answer4());
        question.set("correct", correct());
        question.set("explain1", explain1());
        question.set("explain2", explain2());
        question.set("explain3", explain3());
        question.set("explain4", explain4());
        question.set("image1", image1());
        question.set("image2", image2());
        question.set("image3", image3());
        question.set("image4", image4());
        question.set("page_reference", page_reference());
         
        question.save(null, {
          success: function(question) {
            // Execute any logic that should take place after the object is saved.
            alert('Question saved');
          },
          error: function(question, error) {
            // Execute any logic that should take place if the save fails.
            // error is a Parse.Error with an error code and message.
            alert('Failed to create new object, with error code: ' + error.message);
          }
        });
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
        addQuestion(false); 
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
            alert("Success!")
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
    var usersStart = 0;

    function initViewUsers() {
        
        addUser(false); 
        addQuestion(false); 
        viewQuestions(false); 
        viewUsers(true);
        
        getUsers(0);
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
          },
          error: function(error) {
            alert("Error: " + error.code + " " + error.message);
          }
        });
    }

    function next () {
        getUsers(usersStart);
    }

    function prev () {
        getUsers(usersStart);
    }

    //****************************************
    // viewQuestions
    //****************************************
    var viewQuestions = ko.observable(false);
    var questions = ko.observable([]);
    var questionsStart = 0;
    function initViewQuestions() {
        
        addUser(false); 
        addQuestion(false); 
        viewQuestions(true); 
        viewUsers(false);

        getQuestions(questionsStart);
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
              },
              error: function(error) {
                alert("Error: " + error.code + " " + error.message);
              }
            });
        }

    return {
        loggedIn: loggedIn,
        adminPassword: adminPassword,
        login: login,

        addQuestion: addQuestion,
        initAddQuestion: initAddQuestion,
        title: title,
        answer1: answer1,
        answer2: answer2,
        answer3: answer3,
        answer4: answer4,
        correct: correct,
        explain1: explain1,
        explain2: explain2,
        explain3: explain3,
        explain4: explain4,
        image1: image1,
        image2: image2,
        image3: image3,
        image4: image4,
        page_reference: page_reference,
        save: save,

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

        viewUsers: viewUsers,
        initViewUsers: initViewUsers,
        users: users,
        next: next,
        prev: prev
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
        alert("deleted");
    }

    this.name = name;
    this.email = email;
    this.init = init;
    this.deleteUser = deleteUser;
}
