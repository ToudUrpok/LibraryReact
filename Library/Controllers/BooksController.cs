using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Entities;
using Library.Models;
using Library.Services;
using Library.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBookManagementService _bmService;
        private readonly IMapper _mapper;
        private readonly IHelperService _helperService;

        public BooksController(
            IConfiguration configuration,
            ILogger<BooksController> logger,
            IBookManagementService bmService,
            IMapper mapper,
            IHelperService helperService)
        {
            _configuration = configuration;
            _logger = logger;
            _bmService = bmService;
            _mapper = mapper;
            _helperService = helperService;
        }

        [HttpGet]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult<PageBookModel>> Get(int? offset, int? limit, string sortOrder, string searchString)
        {
            PageBookModel model = new PageBookModel();
            model.TotalBooks = await _bmService.GetAllBooksCountAsync();

            List<Book> books = await _bmService.GetBooksAsync(offset ?? 0, limit ?? 10, sortOrder, searchString);

            foreach (Book book in books)
            {
                BookModel bookModel = new BookModel()
                {
                    Id = book.Id,
                    Name = book.Name,
                    ISBN = book.ISBN,
                    Authors = book.Authors,
                    Genre = book.Genre,
                    Year = book.Year.ToString(),
                    Quantity = book.Quantity.ToString()
                };
                
                model.Books.Add(bookModel);
            }

            return model;
        }

        // GET: api/books/5
        // [HttpGet("{id}"), Name = "Get"]
        [HttpGet("{id}")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult<BookModel>> Get(string id)
        {
            var book = await _bmService.FindBookAsync(id);

            if (book == null)
                return NotFound();

            BookModel model = new BookModel
            {
                Id = book.Id,
                Name = book.Name,
                Authors = book.Authors,
                ISBN = book.ISBN,
                Genre = book.Genre,
                Year = book.Year.ToString(),
                Quantity = book.Quantity.ToString()
            };

            return model;
        }

        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult<BookModel>> Post(BookModel bookModel)
        {
            if (!ModelState.IsValid) // Is automatically done by the [ApiController] controller attribute
                return BadRequest(ModelState);

            short year;
            if (!Int16.TryParse(bookModel.Year, out year))
            {
                return BadRequest("Invalid Year value!");
            }

            if (await _bmService.IsBookExistsAsync(bookModel.ISBN, bookModel.Name, bookModel.Authors, year))
            {
                ModelState.AddModelError("Failure", "Book already exists");
                return BadRequest(ModelState);
            }

            int quantity = 0;
            Int32.TryParse(bookModel.Quantity, out quantity);

            Book book = new Book(bookModel.Name, quantity);
            book.Authors = bookModel.Authors;
            book.Genre = bookModel.Genre;
            book.Year = year;
            book.ISBN = bookModel.ISBN;

            Book result = await _bmService.AddBookAsync(book);

            if (result != null)
            {
                bookModel.Id = result.Id;
                return CreatedAtAction(nameof(Post), new { id = bookModel.Id }, bookModel);
            }
            else
            {
                ModelState.AddModelError("Failure", "Book creation failed");
                return BadRequest(ModelState);
            }
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, BookModel bookModel)
        {
            if (id != bookModel.Id.ToString() || !ModelState.IsValid)
                return BadRequest();

            var book = await _bmService.FindBookAsync(id);

            if (book == null)
                return NotFound();

            short year;
            if (!Int16.TryParse(bookModel.Year, out year))
            {
                return BadRequest("Invalid Year value!");
            }

            bool isUpdated = false;

            if (book.Name != bookModel.Name)
            {
                isUpdated = true;
                book.Name = bookModel.Name;
            }

            if (book.ISBN != bookModel.ISBN)
            {
                isUpdated = true;
                book.ISBN = bookModel.ISBN;
            }

            if (book.Authors != bookModel.Authors)
            {
                isUpdated = true;
                book.Authors = bookModel.Authors;
            }

            if (book.Genre != bookModel.Genre)
            {
                isUpdated = true;
                book.Genre = bookModel.Genre;
            }
            
            if (book.Year != year)
            {
                isUpdated = true;
                book.Year = year;
            }

            if (isUpdated)
            {
                _bmService.UpdateBookAsync(book);
            }

            return NoContent();
        }
    }
}
