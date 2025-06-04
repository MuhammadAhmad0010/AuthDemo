using AuthDemos.Core.DTOs.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Infrastructure.Dapper.BooksRepository
{
    public interface IBookRepository
    {
        Task<List<BooksResponseDTO>> GetAllBooks();
    }
}
