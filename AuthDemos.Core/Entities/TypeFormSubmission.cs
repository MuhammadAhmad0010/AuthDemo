namespace AuthDemos.Core.Entities
{
    public class TypeFormSubmission
    {
        public int Id { get; set; }
        public string FormId { get; set; }
        public string Token { get; set; }
        public string EventId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime LandedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<TypeFormAnswers> Answers { get; set; } = new();
    }
}
