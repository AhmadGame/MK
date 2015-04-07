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

MK.vm.question = function () {
    var self = this;
    var index = 0,
        title = "",
        answer1 = "",
        answer2 = "",
        answer3 = "",
        answer4 = "",
        correct = 0,
        explain1 = "",
        explain2 = "",
        explain3 = "",
        explain4 = "",
        image1 = "",
        image2 = "",
        image3 = "",
        image4 = "",
        page_reference = "",
        correctAnswer = "",
        answeredCorrectly = false;

    var marked = ko.observable(false);
    var userAnswer = ko.observable("");
    var explination = ko.observable("");

    function init(parseObject) {
        self.title = parseObject.get("title");
        self.answer1 = parseObject.get("answer1");
        self.answer2 = parseObject.get("answer2");
        self.answer3 = parseObject.get("answer3");
        self.answer4 = parseObject.get("answer4");
        self.correct = parseObject.get("correct");
        self.explain1 = parseObject.get("explain1");
        self.explain2 = parseObject.get("explain2");
        self.explain3 = parseObject.get("explain3");
        self.explain4 = parseObject.get("explain4");

        if (parseObject.get("image1")) {
            self.image1 = "img/" + parseObject.get("image1");
        }
        if (parseObject.get("image2")) {
            self.image2 = "img/" + parseObject.get("image2");
        }
        if (parseObject.get("image3")) {
            self.image3 = "img/" + parseObject.get("image3");
        }
        if (parseObject.get("image4")) {
            self.image4 = "img/" + parseObject.get("image4");
        }
        self.page_reference = parseObject.get("page_reference");

        if (self.correct === 1) {
            self.correctAnswer = self.answer1;
        }
        else if (self.correct === 2) {
            self.correctAnswer = self.answer2;
        }
        else if (self.correct === 3) {
            self.correctAnswer = self.answer3;
        }
        else if (self.correct === 4) {
            self.correctAnswer = self.answer4;
        }
    }

    function setAnswer(answer) {
        if (parseInt(answer) === self.correct) {
            self.answeredCorrectly = true;
        }

        if (answer === "1") {
            self.explination(self.explain1);
            self.userAnswer(self.answer1);
        } else if (answer === "2") {
            self.explination(self.explain2);
            self.userAnswer(self.answer2);
        } else if (answer === "3") {
            self.explination(self.explain3);
            self.userAnswer(self.answer3);
        } else if (answer === "4") {
            self.explination(self.explain4);
            self.userAnswer(self.answer4);
        } else {
            self.userAnswer("Du svarade inte på frågan.");
        }

    }

    this.init = init;
    this.index = index;
    this.title = title;
    this.answer1 = answer1;
    this.answer2 = answer2;
    this.answer3 = answer3;
    this.answer4 = answer4;
    this.correct = correct;
    this.explain1 = explain1;
    this.explain2 = explain2;
    this.explain3 = explain3;
    this.explain4 = explain4;
    this.image1 = image1;
    this.image2 = image2;
    this.image3 = image3;
    this.image4 = image4;
    this.page_reference = page_reference;
    this.marked = marked;
    this.userAnswer = userAnswer;
    this.correctAnswer = correctAnswer;
    this.explination = explination;
    this.setAnswer = setAnswer;
    this.answeredCorrectly = answeredCorrectly;
};