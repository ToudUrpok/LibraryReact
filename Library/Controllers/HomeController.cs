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
    public class HomeController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBookManagementService _bmService;
        private readonly IMapper _mapper;
        private readonly IHelperService _helperService;

        public HomeController(
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
        public async Task<ActionResult<PageBookItemModel>> Get(int? offset, int? limit, string sortOrder, string searchString)
        {
            PageBookItemModel model = new PageBookItemModel();
            model.TotalBookItems = await _bmService.GetAllBooksCountAsync();

            List<Book> books = await _bmService.GetBooksAsync(offset ?? 0, limit ?? 10, sortOrder, searchString);

            foreach (Book book in books)
            {
                BookItemModel bookModel = new BookItemModel()
                {
                    Id = book.Id,
                    Name = book.Name,
                    ISBN = book.ISBN,
                    Authors = book.Authors,
                    Genre = book.Genre,
                    Year = book.Year.ToString(),
                    IsAvailable = await _bmService.IsBookAvailableAsync(book)
                };

                model.BookItems.Add(bookModel);
            }

            return model;
        }
    }
}
