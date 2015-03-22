/*global $:false */
var MK = MK || {};
MK.vm = {};

MK.vm.Questions = function () {
    var self = this;
    var questions,
        activeIndex = ko.observable(0),
        activeQuestion = ko.observable(null),
        nextQuestion = ko.observable(null),
        previousQuestion = ko.observable(null),
        settingsVisible = ko.observable(true),
        numberOfQuestions = ko.observable(10),
        done = ko.observable(false);

    function next() {
        activeQuestion().userAnswer(getUserAnswer());

        if (!self.nextQuestion()) {
            return;
        }

        self.previousQuestion(activeQuestion());
        self.activeQuestion(nextQuestion());
        self.activeIndex(nextQuestion().index);
        self.nextQuestion(self.questions[activeIndex() + 1]);
    }

    function getUserAnswer() {
        if (document.getElementById('a1').checked) {
            return "1";
        } else if (document.getElementById('a2').checked) {
            return "2";
        } else if (document.getElementById('a3').checked) {
            return "3";
        } else {
            return "4";
        }
    }

    function prev() {
        if (!self.previousQuestion()) {
            return;
        }

        self.nextQuestion(activeQuestion());
        self.activeQuestion(previousQuestion());
        self.activeIndex(previousQuestion().index);
        if (activeIndex() >= 1) {
            self.previousQuestion(self.questions[activeIndex() - 1]);
        } else {
            previousQuestion(null);
        }
    }

    function mark() {
        self.activeQuestion().marked(true);
    }

    function init(data) {
        self.questions = _.map(data, function(questionJson) {
            var question = new MK.vm.question();
            question.init(questionJson);
            return question;
        });

        for (var i = 0; i < self.questions.length; i++) {
            self.questions[i].index = i;
        }

        self.activeIndex(0);
        self.activeQuestion(self.questions[activeIndex()]);
        self.nextQuestion(self.questions[activeIndex() + 1]);
    }

    function takeTest() {
        if (numberOfQuestions() <= 0) {
            window.alert("Ange antalet frågor du vill besvara.");
            return;
        }
        settingsVisible(false);
        MK.server.getJson('/questions/' + numberOfQuestions(), init, function onFailure() {
            document.location.href = './fail.html';
        });
    }

    function correctTest() {
        done(true);
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
    this.correctTest = correctTest;
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
        page_reference;

    var marked = ko.observable(false);
    var userAnswer = ko.observable("0");
	
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
        self.image1 = data.image1;
        self.image2 = data.image2;
        self.image3 = data.image3;
        self.image4 = data.image4;
        self.page_reference = data.page_reference;
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
};
