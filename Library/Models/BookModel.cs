using System.ComponentModel.DataAnnotations;
using System;

namespace Library.Models
{
    public class BookModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string ISBN { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Authors { get; set; }

        [Required]
        [StringLength(200)]
        public string Genre { get; set; }

        [Required]
        public string Year { get; set; }

        public string Quantity { get; set; }
    }
}
