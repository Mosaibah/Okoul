using Okoul.Models;
using Okoul.Models.ViewModels;

namespace Okoul.Services
{
    public interface IAuthorService
    {
        Task<int> AddAuthor(Author author);
        Task<int> DeleteAuthor(int authorId);
        Task<SearchAuthorVM> ListAuthors(SearchAuthorVM model);
        Task<int> UpdateAuthor(Author author);
    }
}