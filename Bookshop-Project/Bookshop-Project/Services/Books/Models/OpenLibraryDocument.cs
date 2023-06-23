namespace Bookshop_Project.Services.Books.Models
{
    public class OpenLibraryDocument
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public List<string> Author_name { get; set; }
        public string Cover_i { get; set; }
    }
}
