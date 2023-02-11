namespace Okoul.Models
{
    public class Quote
    {
        public Quote()
        {
            CreatedAt = DateTime.Now;

        }

        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author {get; set; }
    }
}
