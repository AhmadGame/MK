namespace MkImporter.Models
{
    public class IoQuestion
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public int Correct { get; set; }
        public int UserAnswer { get; set; }
        public string Explain1 { get; set; }
        public string Explain2 { get; set; }
        public string Explain3 { get; set; }
        public string Explain4 { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
    }
}
