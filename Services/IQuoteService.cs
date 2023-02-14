using Okoul.Models;
using Okoul.Models.ViewModels;

namespace Okoul.Services
{
    public interface IQuoteService
    {
        Task<int> AddQuote(Quote quote);
        Task<int> DeleteQuote(int quoteId);
        Task<List<Quote>> GetQuoteByAuthor(int authorId);
        Task<Quote> GetRandomQuote();
        Task<SearchQuotesVM> ListQuotes(SearchQuotesVM model);
        Task<int> UpdateQuote(Quote quote);
    }
}