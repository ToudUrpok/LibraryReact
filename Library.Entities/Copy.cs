using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities
{
    public class Copy
    {
        [Key]
        public Guid InventoryNumber { get; set; }

        public bool IsAvailable { get; set; }

        public Book Book { get; set; }
    }
}
