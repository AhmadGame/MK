using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace Mk.ViewModels
{
    public class FolderViewModel
    {
        public FolderViewModel()
        {
            questions = new List<QuestionViewModel>();
        }

        public string name { get; set; }
        public long id { get; set; }
        public string language { get; set; }
        public List<QuestionViewModel> questions { get; set; }

    }
}