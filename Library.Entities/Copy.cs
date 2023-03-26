using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities
{
    public class Copy
    {
        [Key]
        public int InventoryNumber { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }
    }
}
