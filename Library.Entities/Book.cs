using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities
{
    public class Book
    {
        public Book(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        [Key]
        public Guid Id { get; set; }

        [StringLength(13)]
        public string? ISBN { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public short Year { get; set; }

        [Required]
        public int Quantity { get; set; }

        public List<Genre> Genres { get; set; }

        public List<Author> Authors { get; set; }

        public List<Copy> Copies { get; set; }
    }
}
