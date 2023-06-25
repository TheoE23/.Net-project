using System.ComponentModel.DataAnnotations;

namespace Bookshop_Project.Data
{
    public class BookPriceRequest
    {
        [Key]
        public string key { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
