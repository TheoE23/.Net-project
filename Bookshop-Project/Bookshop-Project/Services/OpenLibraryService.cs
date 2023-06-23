using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bookshop_Project.Models;
using Bookshop_Project.Services.Books.Models;
using Newtonsoft.Json;

namespace Bookshop_Project.Services
{
    public class OpenLibraryService
    {
        private readonly HttpClient _httpClient;

        public OpenLibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookViewModel>> SearchBooks(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                // Handle empty query by returning an empty list
                return new List<BookViewModel>();
            }

            try
            {
                // Perform API request to fetch search results
                var apiUrl = $"https://openlibrary.org/search.json?q={query}";
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OpenLibrarySearchResult>(json);

                var bookViewModels = new List<BookViewModel>();

                // Iterate over the search results and create BookViewModel objects
                foreach (var doc in result.Docs)
                {
                    if (doc.Cover_i != null)
                    {
                        var bookViewModel = new BookViewModel
                        {
                            Title = doc.Title,
                            Author = doc.AuthorName != null ? string.Join(", ", doc.AuthorName) : "Unknown",
                            CoverUrl = $"https://covers.openlibrary.org/b/id/{doc.Cover_i}-L.jpg"
                        };
                        bookViewModels.Add(bookViewModel);
                    }
                    else
                    {
                        var bookViewModel = new BookViewModel
                        {
                            Title = doc.Title,
                            Author = doc.AuthorName != null ? string.Join(", ", doc.AuthorName) : "Unknown",
                            CoverUrl = ""
                        };
                        bookViewModels.Add(bookViewModel);
                    }

                }

                return bookViewModels;
            }
            catch (Exception ex)
            {
                // Handle any errors and return an empty list
                return new List<BookViewModel>();
            }
        }
    }
}
