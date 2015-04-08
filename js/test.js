/*global $:false */
var MK = MK || {};
MK.vm = {};

MK.vm.Test = function () {
    var loggedIn = ko.observable(false);

    // not logged in
    var email = ko.observable(""),
        password = ko.observable(""),
        recover = ko.observable(false);

    function login() {
        Parse.User.logIn(email(), password(), {
          success: function(user) {
            loggedIn(true);
          },
          error: function(user, error) {
            customAlert("danger", "Error: " + error.code + " " + error.message);
          }
        });
    }

    function recoverPassword() {
        recover(true);
        resetAlerts();
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
            if (!user) {
                customAlert("danger", "Error: User " + email() + " not found");
            }
            user.set("password", "potatis");

            user.save(null, {
              success: function(user) {
                recover(false);
                customAlert("info", "Ditt lösenord är återställt till: <strong>potatis</strong>");
              },
              error: function(user, error) {
                customAlert("danger", "Error: " + error.code + " " + error.message);
              }
            });
          },
          error: function(error) {
            customAlert("danger", "Error: " + error.code + " " + error.message);
          }
        });
    }

    function customAlert(type, message) {
        $('#alerts').replaceWith(
            '<div id="alerts"><hr><div class="alert alert-'+ type + '" role="alert">' + message + '</div></div>'
            );
    }

    function resetAlerts() {
        $('#alerts').replaceWith('<div id="alerts"></div>');
    }

    // logged in

    var questions = ko.observable([]),
        activeIndex = ko.observable(0),
        activeQuestion = ko.observable(null),
        nextQuestion = ko.observable(null),
        previousQuestion = ko.observable(null),
        settingsVisible = ko.observable(true),
        numberOfQuestions = ko.observable(10),
        done = ko.observable(false),
        doneDone = ko.observable(false),
        anyMistakes = ko.observable(false),
        result = ko.observable('');

    function next() {
        activeQuestion().setAnswer(getUserAnswer());

        if (!nextQuestion()) {
            return;
        }

        previousQuestion(activeQuestion());
        activeQuestion(nextQuestion());
        activeIndex(nextQuestion().index);
        nextQuestion(questions()[activeIndex() + 1]);
    }

    function getUserAnswer() {
        var a1 = document.getElementById('a1');
        var a2 = document.getElementById('a2');
        var a3 = document.getElementById('a3');
        var a4 = document.getElementById('a4');

        if (a1 && a1.checked) {
            a1.checked = false;
            return "1";
        } else if (a2 && a2.checked) {
            a2.checked = false;
            return "2";
        } else if (a3 && a3.checked) {
            a3.checked = false;
            return "3";
        } else if (a4 && a4.checked) {
            a4.checked = false;
            return "4";
        }
        return "0";
    }

    function prev() {
        if (!previousQuestion()) {
            return;
        }

        nextQuestion(activeQuestion());
        activeQuestion(previousQuestion());
        activeIndex(previousQuestion().index);
        if (activeIndex() >= 1) {
            previousQuestion(questions()[activeIndex() - 1]);
        } else {
            previousQuestion(null);
        }
    }

    function mark() {
        activeQuestion().marked(true);
    }

    function takeTest() {
        if (numberOfQuestions() <= 0) {
            customAlert("warning", "Ange antalet frågor du vill besvara.");
            return;
        }
        settingsVisible(false);

        getFromParse();
        //getRandomFromParse();
    }

    function getFromParse() {
        var q = Parse.Object.extend("question");
        var query = new Parse.Query(q);
        query.limit(numberOfQuestions());
        query.find({
          success: function(results) {
            init(results);
          },
          error: function(error) {
            customAlert("danger", "Error: " + error.code + " " + error.message);
          }
        });
    }

    function getRandomFromParse() {
        var q = Parse.Object.extend("question"),
        count = (new Parse.Query(q)).count(), // total number of questions.
        requestCount = numberOfQuestions(), // number of random cards that you want to query
        query1, query2, randomQuery,
        queries = [],
        i;
        for (i = 0; i < requestCount; i++) {
            query1 = new Parse.Query(q);
            query2 = new Parse.Query(q);
            query1.skip(Math.floor(Math.random() * count));
            query1.limit(1);
            query2.matchesKeyInQuery("objectId", "objectId", query1);
            queries.push(query2);
        }
        randomQuery = Parse.Query.or.apply(this, queries);
        randomQuery.find({
          success: function(results) {
            init(results);
          },
          error: function(error) {
            customAlert("danger", "Error: " + error.code + " " + error.message);
          }
        });
    }

    function showSummary() {
        activeQuestion().setAnswer(getUserAnswer());
        done(true);
    }

    function correctTest() {
        anyMistakes(_.any(questions(), function (q) {
            return !q.answeredCorrectly;
        }));
        var correctCount = _.dropWhile(questions(), function (q) {
            return !q.answeredCorrectly;
        }).length;

        var correctPercent = (correctCount / questions().length) * 100;
        if (correctPercent > 80) {
            result("Du är Godkänd! Du hade" + correctPercent + "% rätt!");
        } else {
            result("Du är inte Godkänd! :( Men du hade " + correctPercent + "% rätt");
        }

        done(false);
        doneDone(true);
    }

    function backToQuestions() {
        done(false);
        activeIndex(0);
        activeQuestion(questions()[activeIndex()]);
        nextQuestion(questions()[activeIndex() + 1]);
    }

    function newTest() {
        doneDone(false);
        settingsVisible(true);
    }

    function init(parseResult) {
        questions(_.map(parseResult, function (parseObject) {
            var question = new MK.vm.question();
            question.init(parseObject);
            return question;
        }));

        for (var i = 0; i < questions().length; i++) {
            questions()[i].index = i;
        }

        activeIndex(0);
        activeQuestion(questions()[activeIndex()]);
        nextQuestion(questions()[activeIndex() + 1]);
    }
    return {
        loggedIn: loggedIn,
        email: email,
        password: password,
        login: login,
        recover: recover,
        recoverPassword: recoverPassword,
        resetPassword: resetPassword,

        questions: questions,
        activeQuestion: activeQuestion,
        activeIndex: activeIndex,
        nextQuestion: nextQuestion,
        previousQuestion: previousQuestion,
        next: next,
        prev: prev,
        mark: mark,
        settingsVisible: settingsVisible,
        numberOfQuestions: numberOfQuestions,
        takeTest: takeTest,
        done: done,
        doneDone: doneDone,
        correctTest: correctTest,
        showSummary: showSummary,
        backToQuestions: backToQuestions,
        anyMistakes: anyMistakes,
        result: result,
        newTest: newTest
    }
};