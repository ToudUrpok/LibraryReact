using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities
{
    public class Genre
    {
        public Genre(string name) 
        { 
            Name = name;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}
