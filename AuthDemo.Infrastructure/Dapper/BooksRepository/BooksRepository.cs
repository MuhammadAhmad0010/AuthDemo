using AuthDemos.Core.DTOs.Author;
using AuthDemos.Core.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Infrastructure.Dapper.BooksRepository
{
    public class BooksRepository : IBookRepository
    {
        private readonly IDbConnection _connection;

        public BooksRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<List<BooksResponseDTO>> GetAllBooks()
        {
            string query =   @"SELECT 
                                  b.Id, b.Title,
                                  a.Name AS AuthorName,
                                  c.Id, c.Name
                              FROM Books b
                              INNER JOIN Author a ON b.AuthorId = a.Id
                              INNER JOIN BookCategory bkc ON bkc.BookId = b.Id
                              INNER JOIN Category c ON c.Id = bkc.CategoryId;";

            var bookDictionary = new Dictionary<int, BooksResponseDTO>();

            var result = await  _connection.QueryAsync<BooksResponseDTO, Category, BooksResponseDTO>(
                query,
                (book, category) =>
                {
                    if (!bookDictionary.TryGetValue(book.Id, out var bookEntry))
                    {
                        bookEntry = book;
                        bookEntry.Categories = new List<Category>();
                        bookDictionary.Add(book.Id, bookEntry);
                    }

                    bookEntry.Categories.Add(category);
                    return bookEntry;
                },
                splitOn: "Id" 
            );

            var books = bookDictionary.Values.ToList();

            return books;
        }
    }
}
