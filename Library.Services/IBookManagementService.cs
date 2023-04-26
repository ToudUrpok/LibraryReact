using Library.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface IBookManagementService
    {
        Task<int> GetAllBooksCountAsync();
        Task<List<Book>> GetAllBooksAsync(string searchString);
        Task<List<Book>> GetBooksAsync(int offset, int limit, string sortOrder, string searchString);
        Task<Book> FindBookAsync(string bookId);
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task<bool> IsBookExistsAsync(string isbn, string name, string authors, short year);
        Task<List<Copy>> AddBookCopiesAsync(Book book, int quantity);
        Task<bool> IsBookAvailableAsync(Book book);
    }
}
