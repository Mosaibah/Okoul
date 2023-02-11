namespace Okoul.Models
{
    public class Author
    {
        public Author()
        {
            CreatedAt = DateTime.Now;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
