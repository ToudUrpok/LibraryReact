using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Entities
{
    public class Session
    {
        public DateTime StartDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public Copy BookCopy { get; set; }

        public ApplicationUser Reader { get; set; }

        public ApplicationUser Librarian { get; set; }
    }
}
