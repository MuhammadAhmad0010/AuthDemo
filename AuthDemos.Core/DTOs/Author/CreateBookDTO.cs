namespace AuthDemos.Core.DTOs.Author
{
    public class CreateBookDTO
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
