// ReSharper disable InconsistentNaming

namespace Mk.ViewModels
{
    public class QuestionViewModel
    {
        public long id { get; set; }
        public string title { get; set; }
        public string answer1 { get; set; }
        public string answer2 { get; set; }
        public string answer3 { get; set; }
        public string answer4 { get; set; }
        public int correct { get; set; }
        public string explain1 { get; set; }
        public string explain2 { get; set; }
        public string explain3 { get; set; }
        public string explain4 { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public int selected { get; set; }
        public string result { get; set; }
        public string userAnswer { get; set; }
    }
}