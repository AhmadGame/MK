using System.Collections.Generic;

namespace Mk.Models
{
    public class Folder
    {
        public Folder()
        {
            Questions = new List<Question>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public List<Question> Questions { get; set; }
        public long QuestionCount { get; set; }
    }
}
