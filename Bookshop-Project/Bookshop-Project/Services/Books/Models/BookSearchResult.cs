namespace Bookshop_Project.Services.Books.Models
{
    using System.Text.Json.Serialization;

    public class BookSearchResult
    {
        [JsonPropertyName("start")]
        public int? Start { get; set; }

        [JsonPropertyName("numFoundExact")]
        public bool? NumFoundExact { get; set; }

        [JsonPropertyName("docs")]
        public List<BookSearchResultItem> Results { get; set; }

        [JsonPropertyName("num_found")]
        public int? NumFound { get; set; }

        [JsonPropertyName("q")]
        public string Query { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }
    }
}
