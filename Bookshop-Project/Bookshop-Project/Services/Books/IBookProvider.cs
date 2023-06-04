namespace Bookshop_Project.Services.Books
{
    using Bookshop_Project.Models;

    public interface IBookProvider
    {
        Task<BookSearchViewModel> SearchBook(string query, int page);
    }
}
