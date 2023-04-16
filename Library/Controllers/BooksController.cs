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
    [Authorize]
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


        [HttpPost]
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
                return CreatedAtAction(nameof(Post), new { id = result.Id }, result);
            }
            else
            {
                ModelState.AddModelError("Failure", "Book creation failed");
                return BadRequest(ModelState);
            }
        }
    }
}
