using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace Mk.ViewModels
{
    public class FolderListViewModel
    {
        public FolderListViewModel()
        {
            languages = new List<string>();
            folders = new List<FolderViewModel>();
        }

        public List<string> languages { get; set; }
        public List<FolderViewModel> folders { get; set; } 
    }
}