using Mk.Models;
using Mk.ViewModels;

namespace Mk.Mappers
{
    public class QuestionMapper
    {
        public Question FromModel(QuestionViewModel question)
        {
            return new Question
            {
                Id = question.id,
                Title = question.title ?? string.Empty,
                Answer1 = question.answer1 ?? string.Empty,
                Answer2 = question.answer2 ?? string.Empty,
                Answer3 = question.answer3 ?? string.Empty,
                Answer4 = question.answer4 ?? string.Empty,
                Correct = question.correct,
                Explain1 = question.explain1 ?? string.Empty,
                Explain2 = question.explain2 ?? string.Empty,
                Explain3 = question.explain3 ?? string.Empty,
                Explain4 = question.explain4 ?? string.Empty,
                Image1 = question.image1 ?? string.Empty,
                Image2 = question.image2 ?? string.Empty,
                Image3 = question.image3 ?? string.Empty,
                Image4 = question.image4 ?? string.Empty
            };
        }

        public QuestionViewModel ToModel(Question question)
        {
            return new QuestionViewModel
            {
                id = question.Id,
                title = question.Title ?? string.Empty,
                answer1 = question.Answer1 ?? string.Empty,
                answer2 = question.Answer2 ?? string.Empty,
                answer3 = question.Answer3 ?? string.Empty,
                answer4 = question.Answer4 ?? string.Empty,
                correct = question.Correct,
                explain1 = question.Explain1 ?? string.Empty,
                explain2 = question.Explain2 ?? string.Empty,
                explain3 = question.Explain3 ?? string.Empty,
                explain4 = question.Explain4 ?? string.Empty,
                image1 = question.Image1 ?? string.Empty,
                image2 = question.Image2 ?? string.Empty,
                image3 = question.Image3 ?? string.Empty,
                image4 = question.Image4 ?? string.Empty
            };
        }

        public Question ToModel(QuestionIoModel question)
        {
            return new Question
            {
                Title = question.title ?? string.Empty,
                Answer1 = question.answer1 ?? string.Empty,
                Answer2 = question.answer2 ?? string.Empty,
                Answer3 = question.answer3 ?? string.Empty,
                Answer4 = question.answer4 ?? string.Empty,
                Correct = question.correct,
                Explain1 = question.explain1 ?? string.Empty,
                Explain2 = question.explain2 ?? string.Empty,
                Explain3 = question.explain3 ?? string.Empty,
                Explain4 = question.explain4 ?? string.Empty,
                Image1 = question.image1 ?? string.Empty,
                Image2 = question.image2 ?? string.Empty,
                Image3 = question.image3 ?? string.Empty,
                Image4 = question.image4 ?? string.Empty
            };
        }
    }
}