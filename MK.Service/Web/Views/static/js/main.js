/*global $:false */
var MK = MK || {};
MK.vm = {};

MK.vm.Questions = function () {
    var self = this;
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

        if (!self.nextQuestion()) {
            return;
        }

        self.previousQuestion(activeQuestion());
        self.activeQuestion(nextQuestion());
        self.activeIndex(nextQuestion().index);
        self.nextQuestion(self.questions()[activeIndex() + 1]);
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
        if (!self.previousQuestion()) {
            return;
        }

        self.nextQuestion(activeQuestion());
        self.activeQuestion(previousQuestion());
        self.activeIndex(previousQuestion().index);
        if (self.activeIndex() >= 1) {
            self.previousQuestion(self.questions()[self.activeIndex() - 1]);
        } else {
            self.previousQuestion(null);
        }
    }

    function mark() {
        self.activeQuestion().marked(true);
    }

    function takeTest() {
        if (self.numberOfQuestions() <= 0) {
            window.alert("Ange antalet frågor du vill besvara.");
            return;
        }
        self.settingsVisible(false);
        MK.server.getJson('/questions/' + numberOfQuestions(), init, function onFailure() {
            document.location.href = './fail.html';
        });
    }

    function showSummary() {
        self.activeQuestion().setAnswer(getUserAnswer());
        self.done(true);
    }

    function correctTest() {
        self.anyMistakes(_.any(questions(), function(q) {
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

        self.done(false);
        self.doneDone(true);
    }

    function backToQuestions() {
        self.done(false);
        self.activeIndex(0);
        self.activeQuestion(self.questions()[activeIndex()]);
        self.nextQuestion(self.questions()[activeIndex() + 1]);
    }

    function init(data) {
        self.questions(_.map(data, function (questionJson) {
            var question = new MK.vm.question();
            question.init(questionJson);
            return question;
        }));

        for (var i = 0; i < self.questions().length; i++) {
            self.questions()[i].index = i;
        }

        self.activeIndex(0);
        self.activeQuestion(self.questions()[activeIndex()]);
        self.nextQuestion(self.questions()[activeIndex() + 1]);
    }

    this.questions = questions;
    this.activeQuestion = activeQuestion;
    this.activeIndex = activeIndex;
    this.nextQuestion = nextQuestion;
    this.previousQuestion = previousQuestion;
    this.next = next;
    this.prev = prev;
    this.mark = mark;
    this.settingsVisible = settingsVisible;
    this.numberOfQuestions = numberOfQuestions;
    this.takeTest = takeTest;
    this.done = done;
    this.doneDone = doneDone;
    this.correctTest = correctTest;
    this.showSummary = showSummary;
    this.backToQuestions = backToQuestions;
    this.anyMistakes = anyMistakes;
    this.result = result;
};

MK.vm.question = function () {
    var self = this;
    var index,
        title,
        answer1,
        answer2,
        answer3,
        answer4,
        correct,
        explain1,
        explain2,
        explain3,
        explain4,
        image1,
        image2,
        image3,
        image4,
        page_reference,
        correctAnswer,
        answeredCorrectly = false;

    var marked = ko.observable(false);
    var userAnswer = ko.observable("");
    var explination = ko.observable("");
	
	function init (data) {
        self.title = data.question;
        self.answer1 = data.answer1;
        self.answer2 = data.answer2;
        self.answer3 = data.answer3;
        self.answer4 = data.answer4;
        self.correct = data.correct;
        self.explain1 = data.explain1;
        self.explain2 = data.explain2;
        self.explain3 = data.explain3;
        self.explain4 = data.explain4;

	    if (data.image1) {
	        self.image1 = "static/img/" + data.image1;
	    }
	    if (data.image2) {
	        self.image2 = "static/img/" + data.image2;
	    }
	    if (data.image3) {
	        self.image3 = "static/img/" + data.image3;
	    }
	    if (data.image4) {
	        self.image4 = "static/img/" + data.image4;
	    }
        self.page_reference = data.page_reference;

        if (self.correct === "1") {
            self.correctAnswer = self.answer1;
        }
        else if (self.correct === "2") {
            self.correctAnswer = self.answer2;
        }
        else if (self.correct === "3") {
            self.correctAnswer = self.answer3;
        }
        else if (self.correct === "4") {
            self.correctAnswer = self.answer4;
        }
	}

    function setAnswer(answer) {
        if (answer === self.correct) {
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
