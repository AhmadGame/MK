// ReSharper disable InconsistentNaming

using System.ComponentModel.DataAnnotations;

namespace Mk.ViewModels
{
    public class QuestionIoModel
    {
        public long id { get; set; }
        public string language { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Question must have a title.")]
        public string title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Question must have at least one answer.")]
        public string answer1 { get; set; }
        public string answer2 { get; set; }
        public string answer3 { get; set; }
        public string answer4 { get; set; }
        [Range(1, 4, ErrorMessage = "Correct answer must be between 1 and 4.")]
        public int correct { get; set; }
        public string explain1 { get; set; }
        public string explain2 { get; set; }
        public string explain3 { get; set; }
        public string explain4 { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
    }
}