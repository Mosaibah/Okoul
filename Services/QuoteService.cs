using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Okoul.Models;
using Okoul.Models.ViewModels;

namespace Okoul.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly OkoulContext _context;

        public QuoteService(OkoulContext context)
        {
            _context = context;
        }

        #region AddQuote
        public async Task<int> AddQuote(Quote quote)
        {
            _context.Add(quote);
            return await _context.SaveChangesAsync();
        }
        #endregion

        public async Task<int> UpdateQuote(Quote quote)
        {
            _context.Update(quote);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Quote>> GetQuoteByAuthor(int authorId)
        {
            return await _context.Quote.Where(c => c.AuthorId == authorId).ToListAsync();
        }

        public async Task<Quote> GetRandomQuote()
        {
            return await _context.Quote.OrderBy(c => Guid.NewGuid()).Take(1).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteQuote(int quoteId)
        {
            Quote quote = new () { Id = quoteId };
            _context.Quote.Remove(quote);
            return await _context.SaveChangesAsync();
        }

        public async Task<SearchQuotesVM> ListQuotes(SearchQuotesVM model)
        {
            List<Quote> quotes = new();
            model.RecordsTotal = await _context.Quote.CountAsync();
            model.RecordsFiltered = 0;

            var countQuotes = await _context.Quote.CountAsync();
            var quotesFiltered = _context.Quote.Include(c => c.Author).AsQueryable();

            if (model.Authors is not null)
            {
                quotesFiltered = quotesFiltered.Where(x => model.Authors.Contains(x.AuthorId.Value));
            }
            if (model.Text is not null)
            {
                quotesFiltered = quotesFiltered.Where(c => c.Text.Contains(model.Text));
            }
            if (model.StartDate is not null)
            {
                quotesFiltered = quotesFiltered.Where(c => c.CreatedAt > model.StartDate);
            }
            if (model.EndDate is not null)
            {
                quotesFiltered = quotesFiltered.Where(c => c.CreatedAt < model.EndDate.Value.AddDays(1));
            }

            model.Quotes = await quotesFiltered.Skip(model.Skip.Value).Take(model.PageSize.Value).ToListAsync();
            model.RecordsFiltered = await quotesFiltered.CountAsync();

            return model;
        }
    }
}
