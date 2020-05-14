using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string image { get; set; }
        public double? Price { get; set; }
        public int? AuthorId { get; set; }
        public AuthorDTO Author { get; set; }
    }
}
