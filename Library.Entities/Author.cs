using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Library.Entities
{
    public class Author
    {
        public Author(string firstName, string midName, string lastName)
        {
            FirstName = firstName;
            MidName = midName;
            LastName = lastName;
        }

        [Key]
        public Guid Id { get; set; }

        [PersonalData]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [PersonalData]
        [MaxLength(100)]
        public string MidName { get; set; }

        [PersonalData]
        [MaxLength(100)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {MidName} {LastName}";
            }
        }

        public List<Book> Books { get; set; }
    }
}
