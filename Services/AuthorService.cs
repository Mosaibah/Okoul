using Okoul.Models;

namespace Okoul.Services
{
    public class AuthorService
    {
        private readonly OkoulContext _context;

        public AuthorService(OkoulContext context)
        {
            _context = context;
        }

        public async Task<int> AddAuthor(Author author)
        {
            _context.Author.Add(author);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAuthor(Author author)
        {
            _context.Author.Update(author);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAuthor(int authorId)
        {
            Author author = new() { Id = authorId };
            _context.Author.Remove(author);
            return await _context.SaveChangesAsync();
        }
    }
}
