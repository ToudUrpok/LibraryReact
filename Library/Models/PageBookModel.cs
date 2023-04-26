using System.Collections.Generic;

namespace Library.Models
{
    public class PageBookModel
    {
        public int TotalBooks { get; set; }
        public List<BookModel> Books { get; set; }
            = new List<BookModel>();
    }

    public class PageBookItemModel
    {
        public int TotalBookItems { get; set; }
        public List<BookItemModel> BookItems { get; set; }
            = new List<BookItemModel>();
    }
}
