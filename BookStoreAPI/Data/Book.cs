﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Data
{
    [Table("Books")]
    public partial class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string image { get; set; }
        public double? Price { get; set; }
        public int? AuthorId { get; set; }
        public Author Author { get; set; }

    }
}