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