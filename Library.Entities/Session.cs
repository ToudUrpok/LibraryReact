using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities
{
    public class Session
    {
        [Key]
        public Guid Id { get; set; }

        public SessionRequest Request { get; set; }

        public DateTime ExpireDate { get; set; }

        public ApplicationUser Librarian { get; set; }
    }
}
