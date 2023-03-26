using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Entities;

namespace Library.Services
{
	public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
	{
        public DbSet<Book> Books { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<SessionRequest> SessionRequests { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public ApplicationDbContext(
			DbContextOptions options,
			IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
		{
		}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.IdentificationNumber)
                .IsUnique();

            builder.Entity<Book>()
                .HasMany(b => b.Copies)
                .WithOne(c => c.Book);
        }
    }
}
