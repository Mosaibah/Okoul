namespace Okoul.Models.ViewModels
{
    public class SearchQuotesVM
    {
        public int? Id { get; set; }
        public List<int>? Authors { get; set; }
        public string? Text { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? SelectedAuthor { get; set; }
        // siplit the models
        public int? PageSize { get; set; }
        public int? Skip { get; set; }
        public string? Draw { get; set; }
        public int? RecordsTotal { get; set; }
        public int? RecordsFiltered { get; set; }
        public List<Quote> Quotes { get; set; }
    }
}
