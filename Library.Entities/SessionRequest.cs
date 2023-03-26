using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Entities
{
    public class SessionRequest
    {
        public bool IsApproved { get; set; }

        public DateTime StartDate { get; set; }

        public Copy BookCopy { get; set; }

        public ApplicationUser Reader { get; set; }
    }
}
