using System.Linq;
using Mk.Models;
using Mk.ViewModels;

namespace Mk.Mappers
{
    public class FolderMapper
    {
        private readonly QuestionMapper _questionMapper;

        public FolderMapper(QuestionMapper questionMapper)
        {
            _questionMapper = questionMapper;
        }

        public Folder FromModel(FolderViewModel folder)
        {
            return new Folder
            {
                Id = folder.id,
                Language = folder.language,
                Name = folder.name,
                Questions = folder.questions?.Select(q => _questionMapper.FromModel(q)).ToList()
            };
        }

        public FolderViewModel ToModel(Folder folder)
        {
            return new FolderViewModel
            {
                id = folder.Id,
                language = folder.Language ?? string.Empty,
                name = folder.Name ?? string.Empty,
                questions = folder.Questions?.Select(q => _questionMapper.ToModel(q)).ToList()
            };
        }
    }
}