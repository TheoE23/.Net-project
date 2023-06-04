namespace Bookshop_Project.Services.Books
{
    using System.Text.Json;

    using Bookshop_Project.Constants;
    using Bookshop_Project.Models;
    using Bookshop_Project.Services.Books.Models;

    using Microsoft.Extensions.Options;

    public class OpenLibraryBookProvider : IBookProvider
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptions<BookDataEndpointOptions> endpointOptions;

        public OpenLibraryBookProvider(IHttpClientFactory httpClientFactory, IOptions<BookDataEndpointOptions> endpointOptions)
        {
            this.httpClientFactory = httpClientFactory;
            this.endpointOptions = endpointOptions;
        }

        public async Task<BookSearchViewModel> SearchBook(string query, int page)
        {
            var httpClient = this.httpClientFactory.CreateClient(HttpClientNames.BookProviderClient);

            var response = await httpClient.GetStringAsync(this.endpointOptions.Value.Search + $"?q={query}&page={page}");

            var searchResult = JsonSerializer.Deserialize<BookSearchResult>(response);

            return new BookSearchViewModel
            {
                Search = query,
                Start = searchResult.Start.Value,
                Found = searchResult.NumFound.Value,
                Page = page,
                Results = searchResult.Results.Select(x => new BookSearchItemViewModel
                {
                    Title = x.Title
                })
            };
        }
    }
}
