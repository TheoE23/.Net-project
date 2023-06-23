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
                var bookViewModels = result?.Docs.Select(doc => new BookViewModel
                {
                    key = doc.Key,
                    Title = doc.Title,
                    Author = doc.Author_name != null ? string.Join(", ", doc.Author_name) : "Unknown",
                    CoverUrl = doc.Cover_i != null ? $"https://covers.openlibrary.org/b/id/{doc.Cover_i}-L.jpg" : ""
                }).ToList();

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
