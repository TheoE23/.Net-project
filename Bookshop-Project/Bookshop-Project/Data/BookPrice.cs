using System.ComponentModel.DataAnnotations;

namespace Bookshop_Project.Data
{
    public class BookPrice
    {
        [Key]
        public string key { get; set; }
        public decimal Price { get; set; }
    }
}
