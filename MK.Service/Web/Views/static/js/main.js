/*global $:false */
var MK = MK || {};
MK.vm = {};

MK.vm.questions = function() {
    var questions = ko.observable([]),
    	activeQuestion = ko.observable(null);

var json = {
	questions: [{
		index: 0,
		title: "Du kommer till en olycksplats. En person som förefaller oskadad uppträder förvirrat. Vad ska du göra?",
		correct: 1,
		answers: [{
			answer: "Ge personen vätska att dricka och tala lugnande."
		},
		{
			answer: "Placera personen i stabilt sidoläge och se till att hon/han håller sig varm."
		},
		{
			answer: "Ta med personen bort från olycksplatsen, se till att hon/han håller sig varm och tala lugnt."
		},
		{
			answer: ""
		}]
	},
	{
		index: 1,
		title: "Var är det förbjudet att parkera och stanna?",
		correct: 2,
		answers: [{
			answer: "Framför en utfart där jag inte hindrar någon."
		},
		{
			answer: "I ett cykelfält."
		},
		{
			answer: "25 meter före en järnvägskorsning."
		},
		{
			answer: "På en huvudled."
		}]
	}]};

    function next () {
    }

    function prev () {
    }

    function init (data) {
    }

    //MK.server.getJson('/admin/datasource/context', init, function onFailure() {
    //    document.location.href = './fail.html';
    //});

	init(json);
    return {
    	questions: questions,
        next: next,
        prev: prev
    };
};

MK.vm.question = function () {
	var title = ko.observable(''),
	answers = ko.observable([]),
	correct = ko.observable(0),
	index = ko.observable(0)
	
	function init (data) {
		title(data.title);
		answers(data.answers);
		correct(data.correct);
	}

	return {
		init: init,
		title: title,
		answers: answers,
		correct: correct,
		index: index
	}
};

MK.vm.loadQuestions = function  (onSuccess, onFailure) {
	
}