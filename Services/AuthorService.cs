using Microsoft.EntityFrameworkCore;
using Okoul.Models;
using Okoul.Models.ViewModels;

namespace Okoul.Services
{
    public class AuthorService : IAuthorService
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

        public async Task<SearchAuthorVM> ListAuthors(SearchAuthorVM model)
        {
            List<Author> author = new();
            model.RecordsTotal = await _context.Author.CountAsync();

            model.RecordsFiltered = 0;

            var countQuotes = await _context.Author.CountAsync();
            var authorsFiltered = _context.Author.AsQueryable();

            if (model.AuthorIds is not null)
            {
                authorsFiltered = authorsFiltered.Where(x => model.AuthorIds.Contains(x.Id));
            }
            if (model.StartDate is not null)
            {
                authorsFiltered = authorsFiltered.Where(c => c.CreatedAt > model.StartDate);
            }
            if (model.EndDate is not null)
            {
                authorsFiltered = authorsFiltered.Where(c => c.CreatedAt < model.EndDate.Value.AddDays(1));
            }

            model.Authors = await authorsFiltered.Skip(model.Skip.Value).Take(model.PageSize.Value).ToListAsync();
            model.RecordsFiltered = await authorsFiltered.CountAsync();

            return model;
        }
    }
}
