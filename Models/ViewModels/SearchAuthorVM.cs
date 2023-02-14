namespace Okoul.Models.ViewModels
{
    public class SearchAuthorVM
    {
        public int? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? SelectedAuthor { get; set; }
        public int? PageSize { get; set; }
        public int? Skip { get; set; }
        public string? Draw { get; set; }
        public int? RecordsTotal { get; set; }
        public int? RecordsFiltered { get; set; }
        public List<Author>? Authors { get; set; }
        public List<int>? AuthorIds { get; set; }
    }
}
