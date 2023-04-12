using System.ComponentModel.DataAnnotations;
using System;

namespace Library.Models
{
    public class BookModel
    {
        public Guid Id { get; set; }

        [StringLength(13)]
        public string ISBN { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public short Year { get; set; }

        [StringLength(200)]
        public string Authors { get; set; }

        [StringLength(200)]
        public string Genre { get; set; }

        public int Quantity { get; set; }
    }
}
