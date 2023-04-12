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
            List<BookModel> books = new List<BookModel>()
            {
                new BookModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Idiot",
                    ISBN = "9780733426094",
                    Authors = "Fiodor Michailovich Dostoevskii",
                    Genre = "Novel",
                    Year = 2005,
                    Quantity = 0
                },
                new BookModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Steps",
                    ISBN = "9780733426095",
                    Authors = "Test Author Name",
                    Genre = "Poem",
                    Year = 2003,
                    Quantity = 0
                },
                new BookModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cars",
                    ISBN = "9780733426096",
                    Authors = "Test Author Name2",
                    Genre = "Detective story",
                    Year = 2003,
                    Quantity = 0
                },
                new BookModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Bikes",
                    ISBN = "9780733426097",
                    Authors = "Test Author Name3",
                    Genre = "Encyclopedia",
                    Year = 2003,
                    Quantity = 0
                },
                new BookModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cooking",
                    ISBN = "9780733426098",
                    Authors = "Test Author Name4",
                    Genre = "Culinary",
                    Year = 2003,
                    Quantity = 0
                }
            };
            model.Books = books;
            model.TotalBooks = books.Count;

            return model;
        }

        // GET: BooksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BooksController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<BookModel>> Post(BookModel bookModel)
        {
            if (!ModelState.IsValid) // Is automatically done by the [ApiController] controller attribute
                return BadRequest(ModelState);

            if (await _bmService.IsBookExistsAsync(bookModel.ISBN, bookModel.Name, bookModel.Authors, bookModel.Year))
            {
                ModelState.AddModelError("Failure", "Book already exists");
                return BadRequest(ModelState);
            }

            Book book = _mapper.Map<Book>(bookModel);
            Book result = await _bmService.AddBookAsync(book);

            if (result != null)
            {
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            else
            {
                ModelState.AddModelError("Failure", "Book creation failed");
                return BadRequest(ModelState);
            }
        }

        // GET: BooksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BooksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BooksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
