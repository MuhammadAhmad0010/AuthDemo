using AuthDemo.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthDemos.Core.Entities;
using AuthDemos.Core.DTOs.Author;
using AuthDemos.Core.DTOs.Response;
using AuthDemo.Infrastructure.Dapper.BooksRepository;
using System.Collections.Immutable;


namespace AuthDemo.Infrastructure.BusinessLogic
{
    public class AuthorBusinessLogic : IAuthorBusinessLogic
    {
        private readonly IDapperRepository<Author> _authorDapperRepository;
        private readonly IDapperRepository<Books> _booksDapperRepository;
        private readonly IDapperRepository<Category> _categoryDapperRepository;
        private readonly IDapperRepository<BookCategory> _bookCategoryDapperRepository;
        private readonly IBookRepository _bookRepository;
        public AuthorBusinessLogic(IDapperRepository<Author> authorDapperRepository,
            IDapperRepository<Books> booksDapperRepository,
            IDapperRepository<Category> categoryDapperRepository,
            IDapperRepository<BookCategory> bookCategoryDapperRepository,
            IBookRepository bookRepository)
        {
            _authorDapperRepository = authorDapperRepository;
            _booksDapperRepository = booksDapperRepository;
            _categoryDapperRepository = categoryDapperRepository;
            _bookCategoryDapperRepository = bookCategoryDapperRepository;
            _bookRepository = bookRepository;
        }

        public async Task<ResponseDTO> CreateAuthor(CreateAuthorDTO createAuthorDTO)
        {
            ResponseDTO response = new();
            try
            {
                Author author = new() { Name = createAuthorDTO.Name };
                await _authorDapperRepository.InsertAsync(author);

                response.Message = "Author created successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> CreateBooks(CreateBookDTO createBookDTO)
        {
            ResponseDTO response = new();
            try
            {
                Books books = new() { Title = createBookDTO.Title, AuthorId = createBookDTO.AuthorId };
                var bookId = await _booksDapperRepository.InsertAsync(books);

                foreach (var categoryIds in createBookDTO.CategoryIds)
                {
                    BookCategory bookCategory = new() { BookId = bookId, CategoryId = categoryIds };
                    await _bookCategoryDapperRepository.InsertAsync(bookCategory);
                }

                response.Message = "Books created successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> CreateCategory(CreateCategoryDTO createCategory)
        {
            ResponseDTO response = new();
            try
            {
                Category category = new() { Name = createCategory.Name };
                await _categoryDapperRepository.InsertAsync(category);

                response.Message = "Category created successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> GetAllAuthors()
        {
            ResponseDTO response = new();
            try
            {
                var authors = await _authorDapperRepository.GetAllAsync();
                response.Data = authors;

                response.Message = "List fetched successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> GetAllBooks()
        {
            ResponseDTO response = new();
            try
            {
                var books = await _bookRepository.GetAllBooks();
                response.Data = books;

                response.Message = "List fetched successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> GetAllCategories()
        {
            ResponseDTO response = new();
            try
            {
                var categories = await _categoryDapperRepository.GetAllAsync();
                response.Data = categories;

                response.Message = "List fetched successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> UpdateAuthor(UpdateAuthorDTO updateAuthorDTO)
        {
            ResponseDTO response = new();
            try
            {
                var author = await _authorDapperRepository.GetByIdAsync(updateAuthorDTO.Id);
                if (author is null)
                    throw new ArgumentException("Author Id is not valid");

                author.Name = updateAuthorDTO.Name;
                await _authorDapperRepository.UpdateAsync(author);

                response.Message = "Author updated successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> DeleteBooks(int bookId)
        {
            ResponseDTO response = new();
            try
            {
                var bookCategories = await _bookCategoryDapperRepository.GetByColumnAsync(nameof(BookCategory.BookId), bookId);
                if (bookCategories != null && bookCategories.Any())
                {
                    foreach (var id in bookCategories.Select(x => x.Id))
                    {
                        await _bookCategoryDapperRepository.DeleteAsync(id);
                    }
                }

                await _booksDapperRepository.DeleteAsync(bookId);
                response.Message = "Books deleted successfully.";

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
