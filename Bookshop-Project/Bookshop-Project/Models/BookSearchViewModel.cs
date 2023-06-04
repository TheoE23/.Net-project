namespace Bookshop_Project.Models
{
    public class BookSearchViewModel
    {
        public int Page { get; set; }

        public string Search { get; set; }

        public int Start { get; set; }

        public int Found { get; set; }

        public IEnumerable<BookSearchItemViewModel> Results { get; set; }
    }
}
