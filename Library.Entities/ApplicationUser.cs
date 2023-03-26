using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Entities
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
        [PersonalData]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [PersonalData]
        [MaxLength(100)]
        public string MidName { get; set; }

        [PersonalData]
        [MaxLength(100)]
        public string LastName { get; set; }

        [PersonalData]
        public DateTime DateOfBirth { get; set; }

        [PersonalData]
        [StringLength(14)]
        public string IdentificationNumber { get; set; }

        public bool ApplicationEditingAllowed { get; set; } = true;

        public bool Approved { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {MidName} {LastName}";
            }
        }
	}
}
