using System;

namespace Library.Entities
{
    public class Session
    {
        public SessionRequest Request { get; set; }

        public DateTime ExpireDate { get; set; }

        public ApplicationUser Librarian { get; set; }
    }
}
