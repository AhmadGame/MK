using System.Collections.Generic;

namespace MkImporter.Models
{
    public class IoFolder
    {
        public long Id { get; set; }
        public string Language { get; set; }
        public List<IoQuestion> Questions { get; set; } 
    }
}
