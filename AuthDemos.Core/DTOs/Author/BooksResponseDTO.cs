using AuthDemos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemos.Core.DTOs.Author
{
    public class BooksResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName {  get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
