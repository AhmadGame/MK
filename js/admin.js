/*global $:false */
var MK = MK || {};
MK.vm = {};

MK.vm.Admin = function () {
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
            //alert('New object created with objectId: ' + gameScore.id);'
            document.location.reload();
          },
          error: function(question, error) {
            // Execute any logic that should take place if the save fails.
            // error is a Parse.Error with an error code and message.
            alert('Failed to create new object, with error code: ' + error.message);
          }
        });
    }

    return {
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
        save: save
    }
}