using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Entities
{
    public class SessionRequest
    {
        [Key]
        public Guid Id { get; set; }

        public bool IsApproved { get; set; }

        public DateTime StartDate { get; set; }

        public Copy BookCopy { get; set; }

        public ApplicationUser Reader { get; set; }
    }
}
