using Library.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class BookManagementService : IBookManagementService
    {
        private readonly ApplicationDbContext _context;

        public BookManagementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetAllBooksCountAsync()
        {
            return await _context.Books.AsNoTracking().CountAsync();
        }

        public async Task<List<Book>> GetAllBooksAsync(string searchString)
        {
            var books = GetBooksSearched(searchString);

            return await books.ToListAsync();
        }

        public async Task<List<Book>> GetBooksAsync(int offset, int limit, string sortOrder, string searchString)
        {
            offset = offset < 0 ? 0 : offset;
            limit = limit < 0 ? 0 : limit;


            var pageBooks = GetBooksSearched(searchString);

            switch (sortOrder)
            {
                case "Name":
                    pageBooks = pageBooks.OrderBy(b => b.Name);
                    break;
                case "Name_desc":
                    pageBooks = pageBooks.OrderByDescending(b => b.Name);
                    break;
                case "Author":
                    pageBooks = pageBooks.OrderBy(u => u.Authors);
                    break;
                case "Author_desc":
                    pageBooks = pageBooks.OrderByDescending(u => u.Authors);
                    break;
                case "Genre":
                    pageBooks = pageBooks.OrderBy(u => u.Genre);
                    break;
                case "Genre_desc":
                    pageBooks = pageBooks.OrderByDescending(u => u.Genre);
                    break;
                default:
                    pageBooks = pageBooks.OrderBy(b => b.Name);
                    break;
            }

            pageBooks = pageBooks.Skip(offset).Take(limit);

            return await pageBooks.ToListAsync();
        }

        public async Task<Book> FindBookAsync(string bookId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id.ToString() == bookId);

            return book;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            if (IsBookExistsInternal(book.ISBN, book.Name, book.Authors, book.Year))
            {
                return null;
            }

            var result = await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();

            if (book.Quantity > 0)
            {
                await AddBookCopiesAsync(result.Entity, result.Entity.Quantity);
            }

            return result.Entity;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var curBook = await _context.Books.FindAsync(book.Id);

            if (curBook != null )
            {
                if (book.ISBN != curBook.ISBN && _context.Books.Where(b => b.ISBN == book.ISBN).Any())
                {
                    return null;
                }

                curBook.Name = book.Name;
                curBook.ISBN = book.ISBN;
                curBook.Authors = book.Authors;
                curBook.Genre = book.Genre;
                curBook.Year = book.Year;
                curBook.Quantity = book.Quantity;

                await _context.SaveChangesAsync();

                return curBook;
            }

            return null;
        }

        public async Task<bool> IsBookExistsAsync(string isbn, string name, string authors, short year)
        {
            return IsBookExistsInternal(isbn, name, authors, year);
        }

        public async Task<List<Copy>> AddBookCopiesAsync(Book book, int quantity)
        {
            List<Copy> result = new List<Copy>();

            for (int i = 0; i < quantity; i++)
            {
                var copy = new Copy()
                {
                    Book = book,
                    IsAvailable = true
                };

                var copyEntry = await _context.Copies.AddAsync(copy);
                result.Add(copyEntry.Entity);
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> IsBookAvailableAsync(Book book)
        {
            return _context.Copies.Where(c => c.Book.Id == book.Id && c.IsAvailable).Any();
        }

        private bool IsBookExistsInternal(string isbn, string name, string authors, short year)
        {
            bool result = _context.Books.Where(b => b.ISBN == isbn).Any()
                || _context.Books.Where(b => b.Name == name && b.Authors == authors && b.Year == year).Any();

            return result;
        }

        private IQueryable<Book> GetBooksSearched(string searchString)
        {
            var books = _context.Books.AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
                books = books.Where(book => (book.Name.Contains(searchString)
                    || book.Authors.Contains(searchString)
                    || book.Genre.Contains(searchString)));

            return books;
        }
    }
}
