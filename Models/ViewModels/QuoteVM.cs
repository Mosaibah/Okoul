namespace Okoul.Models.ViewModels
{
    public class QuoteVM
    {
        public QuoteVM()
        {
            CreatedAt = DateTime.Now;
        }

        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
    }
}
