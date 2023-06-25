using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bookshop_Project.Models;
using Bookshop_Project.Services.Books.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bookshop_Project.Services
{
    public class OpenLibraryService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;

        public OpenLibraryService(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<List<BookViewModel>> SearchBooks(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new List<BookViewModel>();
            }

            try
            {
                var apiUrl = $"https://openlibrary.org/search.json?q={query}";
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OpenLibrarySearchResult>(json);
                var bookPrices = await _context.BookPrices.ToListAsync();
                var bookViewModels = result?.Docs.Select(async doc =>
                {
                    var bookViewModel = new BookViewModel
                    {
                        key = doc.Key,
                        Title = doc.Title,
                        Author = doc.Author_name != null ? string.Join(", ", doc.Author_name) : "Unknown",
                        CoverUrl = doc.Cover_i != null ? $"https://covers.openlibrary.org/b/id/{doc.Cover_i}-L.jpg" : ""
                    };

                    // Fetch the price from the database
                    var bookPrice = bookPrices.FirstOrDefault(x=>x.key==doc.Key);
                    if (bookPrice != null)
                    {
                        bookViewModel.Price = bookPrice.Price;
                    }

                    return bookViewModel;
                }).ToList();
                var fetchedBookViewModels = await Task.WhenAll(bookViewModels);


                return fetchedBookViewModels.ToList();
            }
            catch (Exception ex)
            {
                return new List<BookViewModel>();
            }
        }

    }
}
